using System;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using COCOA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ServiceModel;

/// <summary>
/// Webscraper grabs schedule information and keeps
/// each lecture in a "Lecture"-object.
/// Takes "username" from 1024 in the constructor.
/// </summary>
[RequireHttps]
public class WebScraper
{
    private string user, schedule;
    private List<Lecture> lectures;

    /// <summary>
    /// A class containing needed information about each lecture
    /// </summary>
        
    public class Lecture
    {
        private string day, time, course, startTime;
        public Lecture(string course, string day, string time)
        {
            this.day = day;
            this.time = time;
            this.course = course;
            this.startTime = time.Split('-')[0];
        }
        /// <summary>
        /// Returns a json-formatted "NextLecture"-object, found in Models.
        /// </summary>
        [AllowAnonymous]
        public string getLectureJson()
        {
            string json = JsonConvert.SerializeObject(
               new NextLecture {course = this.course, day=this.day,time=this.time});
            return json;
        }
        /// <summary>
        /// Get the date of a lecture
        /// </summary>
        public string getDay() { return this.day; }
        /// <summary>
        /// Convert time to the format: HH.MM.SS
        /// </summary>
        public string getFormattedTime() { return this.startTime.Replace(':', '.') + ".00"; }

        public override string ToString()
        {
            return "Class: " + this.course + "\nDay: " + this.day + "\nTime: " + this.time + "\n";
        }
    }

    /// <summary>
    ///  Parses the string retrieved from scraping the timeplan website
    /// </summary>
    /// <returns>a prettified string, containing relevant data</returns>
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
        string year = DateTime.Now.Year.ToString();
        var html = new HtmlDocument();
        this.schedule = "https://ntnu.1024.no/" + year + "/var/" + user + "/";
        html.LoadHtml(new WebClient().DownloadString(schedule));
        var root = html.DocumentNode;
        // select nodes under table id=lecture and under the tag tbody
        foreach (HtmlNode node in root.SelectNodes("//table[@id='lectures']//tbody"))
        {
            foreach (var child in node.ChildNodes)
            {
                // ORIGINAL:    Tjenester og nett onsdag 18:15-20:00   F1  A~ving MTDT, MTIA~T, MTKOM, MTTK, BIT, MTENERG, MIENERG 2-14, 16
                string formatted = this.parseLine(child.InnerText);
                // PARSED:      Tjenester og nett onsdag 18:15-20:00
                string[] splitted = formatted.Split();
                if (splitted.Length > 2) // valid list of subject, time and day.
                {
                    string time = splitted[splitted.Length - 1];
                    string day = splitted[splitted.Length - 2];
                    formatted = formatted.Replace(time, "");
                    formatted = formatted.Replace(day, "");
                    string subject = formatted.Trim();
                    day = FirstCharToUpper(day);
                    lectures.Add(new Lecture(subject, day, time));
                }
            }
        }
        //this.getJson();
    }

    /// <summary>
    /// These dictionaries are used to find the date based on a given day name.
    /// This has to be done as the web scraping only returns the weekday, not date.
    /// </summary>
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

    /// <summary>
    /// Based on the weekday, calculate the date
    /// </summary>
    /// <returns>JSON object containing information about the next lecture</returns>
    public string getNextLecture()
    {
        var currentTime = DateTime.Now;
        //currentTime = currentTime.AddDays(5); //used for debugging
        int todayValue = this.dayValues[currentTime.DayOfWeek.ToString()];
        // set a default high timespan. This is later used to check hours until next lecture.
        double minTimeSpan = 99999;
        // set the default next lecture to the first one in line.
        Lecture nextLecture = this.lectures[0];
        // format may vary based on OS
        // #TODO: grab format from system
        string format = "dd/MM/yyyy HH.mm.ss";
        foreach (var lecture in this.lectures)
        {
            int dayDiff = this.dayValuesNorwegian[lecture.getDay()] - todayValue;
            if (dayDiff >= 0) // if the day is in the future...
            {
                string lectureTimeDate = findLectureDate(currentTime, dayDiff) + " " + lecture.getFormattedTime();
                DateTime lectureDateTime = DateTime.ParseExact(lectureTimeDate, format, null);
                TimeSpan dayTimeDiff = lectureDateTime - currentTime;
                // if there's fewer hours until this lecture than the previous one.
                if (dayTimeDiff.TotalHours > 0 && dayTimeDiff.TotalHours < minTimeSpan)
                {
                    // update temporary next lecture
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