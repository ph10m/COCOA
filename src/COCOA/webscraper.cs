using System;
using HtmlAgilityPack;
using System.Net;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebScraper{

    private string user, schedule;
    private List<string> courses;

    class Course
    {
        private string name;
        private List<string> lectures;
        Course(string coursename)
        {

        }
        public void addLecture(string day, string time)
        {
            this.lectures.Add(day+" at "+time);
        }
    }
    
    public WebScraper(string user){
        string year = DateTime.Now.Year.ToString();
        courses = new List<string>();
        var html = new HtmlDocument();
        this.user = user;
        this.schedule = "https://ntnu.1024.no/"+year+"/var/"+user+"/";
        html.LoadHtml(new WebClient().DownloadString(schedule));
        var root = html.DocumentNode;
        //var lectureTable = root.Descendants().Where(nameof => nameof.GetAttributeValue("id", "").Equals("lectures"));

        //foreach (HtmlNode node in html.DocumentNode.SelectNodes("//table[@id='lectures']"))
        //{
        //    Console.WriteLine(node.InnerText);
        //    Console.WriteLine(node.InnerHtml);
        //}
        //var lectureTable = root.Descendants().Where(nameof => nameof.GetAttributeValue("id", "").Equals("lectures"));
        //var lecturesInTable = root.Descendants().Where(AttributeTargets)
        //foreach (var tableval in lectureTable)
        //{
        //    Console.WriteLine(tableval.InnerText);
        //}

        var nodes = root.Descendants().Where(n => n.Attributes.Any(a => a.Value.Contains("lecture lecture-")));
        foreach (var node in nodes)
        {
            //Console.WriteLine(node.InnerHtml);
            string courseAndTime = node.GetAttributeValue("title", "");
            //e.g. Kommunikasjon - Tjenester og nett 10:15-12:00
            int timeIndex = 0;
            for (int i = 0; i < courseAndTime.Length; i++){
                if (Char.IsDigit(courseAndTime[i])){
                    timeIndex = i - 1;
                    break;
                }
            }
            string courseName = courseAndTime.Substring(0, timeIndex);
            if (!courses.Contains(courseName)) courses.Add(courseName);
            string courseTime = courseAndTime.Substring(timeIndex + 1);
            Console.WriteLine("Course name: " + courseName + " is at: " + courseTime);
            var innerlist = node.InnerHtml.Split();
            foreach (var item in innerlist)
            {
                if (item.StartsWith("href"))
                {
                    string mazemap = item.Substring(6);
                    mazemap = mazemap.Remove(mazemap.Length - 2);
                    Console.WriteLine(mazemap);
                }
            }
            Console.WriteLine("-------------------------");
        }
        this.getJson();
        //foreach (var course in this.courses)
        //{
        //    Console.WriteLine(course);
        //    var courseNodes = root.Descendants().Where(n => n.Attributes.Any(a => a.Value.Contains(course)));
        //    foreach (var node in courseNodes)
        //    {
        //        Console.WriteLine(node.InnerHtml);
        //    }
        //}
    }
    private void getJson()
    {

    }
}
