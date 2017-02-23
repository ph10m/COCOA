using System;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class WebScraper{

    private string user, schedule;
    private List<string> courseNames;
    private List<Lecture> lectures;

    public class Lecture
    {
        private string day { get; set; }
        private string time { get; set; }
        private string course {get;set;}
        private string startTime { get; set; }
        private string endTime { get; set; }
        public Lecture(string course, string day, string time)
        {
            this.day = day;
            this.time = time;
            this.course = course;

            string[] timeSplit = time.Split('-');
            this.startTime = timeSplit[0];
            this.endTime = timeSplit[1];
        }
        public string getLecture()
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

    public WebScraper(string user) {
        lectures = new List<Lecture>();
        courseNames = new List<string>();
        string year = DateTime.Now.Year.ToString();
        var html = new HtmlDocument();
        this.user = user;
        this.schedule = "https://ntnu.1024.no/"+year+"/var/"+user+"/";
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
                    day = FirstCharToUpper(day);
                    formatted = formatted.Replace(time, "");
                    formatted = formatted.Replace(day, "");
                    string subject = formatted.Trim();
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

    private Lecture getNextLecture()
    {
        var currentTime = DateTime.Now;
        var day = currentTime.DayOfWeek;
        var time = currentTime.TimeOfDay;
        Console.WriteLine("It's now " + day + ", " + time);

        return null;
    }

    private void getJson()
    {
        foreach (var lect in this.lectures)
        {
            Console.WriteLine(lect.getLecture());
        }
    }
}
