using System;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class WebScraper
{

    private string user, schedule;
    private List<string> courseNames;
    private List<Lecture> lectures;

    public class Lecture
    {
        private string day;
        private string time;
        private string course;
        private string startTime;
        private string endTime;
        public Lecture(string course, string day, string time)
        {
            this.day = day;
            this.time = time;
            this.course = course;

            string[] timeSplit = time.Split('-');
            this.startTime = timeSplit[0];
            this.endTime = timeSplit[1];
        }
        public string getLectureJson()
        {
            string json = JsonConvert.SerializeObject(new
            {
                lecture = new List<JsonLecture>()
                {
                    new JsonLecture {course = this.course, day=this.day, time=this.time }
                }
            });
            return json;
        }
        public int getStartTime()
        {
            // return sthe amount of minutes in the start-time of a lecture
            // for easy comparison
            var tmp = startTime.Split(':');
            int x = Int32.Parse(tmp[0]) * 60 + Int32.Parse(tmp[1]);
            Console.WriteLine("was: " + this.startTime + "becomes in mins: " + x);
            return x;
        }
        public string getDay() { return this.day; }
        public string getTime() { return this.startTime; }
        public string getFormattedTime() { return this.startTime.Replace(':', '.') + ".00"; }

        public override string ToString()
        {
            return "Class: " + this.course + "\nDay: " + this.day + "\nTime: " + this.time + "\n";
        }
    }
    public class JsonLecture
    {
        public string day { get; set; }
        public string time { get; set; }
        public string course { get; set; }
    }

    private string parseLine(string text)
    {
        text = text.Trim();
        int split_index = text.IndexOf("0   ");  // the end of the given time + trailing spaces
        return text.Substring(0, split_index + 1);
    }

    private static string FirstCharToUpper(string input)
    {
        if (String.IsNullOrEmpty(input)) throw new ArgumentException("Invalid input!");
        return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
    }

    public WebScraper(string user)
    {
        lectures = new List<Lecture>();
        courseNames = new List<string>();
        string year = DateTime.Now.Year.ToString();
        var html = new HtmlDocument();
        this.user = user;
        this.schedule = "https://ntnu.1024.no/" + year + "/var/" + user + "/";
        html.LoadHtml(new WebClient().DownloadString(schedule));
        var root = html.DocumentNode;

        // select nodes under table id=lecture and under the tag tbody
        foreach (HtmlNode node in root.SelectNodes("//table[@id='lectures']//tbody"))
        {
            //Console.WriteLine(node.InnerText);
            foreach (var child in node.ChildNodes)
            {
                //Console.WriteLine(child.InnerText);
                // Tjenester og nett onsdag 18:15-20:00   F1  A~ving MTDT, MTIA~T, MTKOM, MTTK, BIT, MTENERG, MIENERG 2-14, 16
                string formatted = this.parseLine(child.InnerText);
                // Tjenester og nett onsdag 18:15-20:00
                string[] splitted = formatted.Split();
                if (splitted.Length > 2) // valid list of subject, time and day.
                {
                    string time = splitted[splitted.Length - 1];
                    string day = splitted[splitted.Length - 2];
                    formatted = formatted.Replace(time, "");
                    formatted = formatted.Replace(day, "");
                    string subject = formatted.Trim();
                    day = FirstCharToUpper(day);
                    //if (!this.courseNames.Contains(subject))
                    //{
                    //    courseNames.Add(subject);
                    //}
                    //Console.WriteLine("Subject:\t" + subject + "\nDay:\t" + day + "\nTime:\t" + time);
                    lectures.Add(new Lecture(subject, day, time));
                }
            }
        }

        this.getJson();
        this.getNextLecture();
    }

    Dictionary<string, int> dayValues = new Dictionary<string, int>()
    {
        {"Monday",      1 },
        {"Tuesday",     2 },
        {"Wednesday",   3 },
        {"Thursday",    4 },
        {"Friday",      5 },
        {"Saturday",    -1 },   // if it's saturday, monday (1) - saturday(-1) = 2 days till monday
        {"Sunday",      0 }     // like above, if it's sunday, monday(1)-sunday(0) = 1 day till monday
    };
    Dictionary<string, int> dayValuesNorwegian = new Dictionary<string, int>()
    {
        {"Mandag",      1 },
        {"Tirsdag",     2 },
        {"Onsdag",      3 },
        {"Torsdag",     4 },
        {"Fredag",      5 }
    };
    private string findLectureDate(DateTime now, int daysDifference)
    {
        return now.AddDays(daysDifference).ToShortDateString();
    }

    private string getNextLecture()
    {
        var currentTime = DateTime.Now;
        //currentTime = currentTime.AddDays(5); //used for debugging
        int todayValue = this.dayValues[currentTime.DayOfWeek.ToString()];
        //TimeSpan minDiff = new TimeSpan(999);   //set a default high timespan
        double minTimeSpan = 99999;
        Lecture nextLecture = this.lectures[0];
        string format = "dd/MM/yyyy HH.mm.ss";
        foreach (var lecture in this.lectures)
        {
            int dayDiff = this.dayValuesNorwegian[lecture.getDay()] - todayValue;
            if (dayDiff >= 0) // if the day is in the future...
            {
                string lectureTimeDate = findLectureDate(currentTime, dayDiff) + " " + lecture.getFormattedTime();
                //Console.WriteLine(lectureTimeDate);
                DateTime lectureDateTime = DateTime.ParseExact(lectureTimeDate, format, null);

                TimeSpan dayTimeDiff = lectureDateTime - currentTime;
                if (dayTimeDiff.TotalHours > 0 && dayTimeDiff.TotalHours < minTimeSpan)
                {
                    //Console.WriteLine("Changed next lecture to " + lecture.getLectureJson());
                    minTimeSpan = dayTimeDiff.TotalHours;
                    nextLecture = lecture;
                }
            }
        }
        Console.WriteLine("Your next lecture is:\n" + nextLecture.getLectureJson());
        return nextLecture.getLectureJson();
    }

    private void getJson()
    {
        String lectureJson = "";
        foreach (var lect in this.lectures)
        {
            lectureJson += lect.getLectureJson() + "\n";
        }
        Console.WriteLine(lectureJson);
    }
}