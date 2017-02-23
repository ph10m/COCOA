using System;
using HtmlAgilityPack;
using System.Net;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Day.Models;
using COCOA.Models;

public class WebScraper{

    private string user, schedule;

    //class Course
    //{
    //    private string name;
    //    private List<string> lectures;
    //    Course(string coursename)
    //    {

    //    }
    //    public void addLecture(string day, string time)
    //    {
    //        this.lectures.Add(day+" at "+time);
    //    }
    //}
    
    private string parseLine(string text)
    {
        text = text.Trim();
        int split_index = text.IndexOf("0   ");  // the end of the given time + trailing spaces
        return text.Substring(0, split_index + 1);
    }

    public WebScraper(string user) {
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
                formatted.Split().ToList().ForEach(Console.WriteLine);
                string[] splitted = formatted.Split();
                if (splitted.Length > 2) // valid list of subject, time and day.
                {
                    string time = splitted[splitted.Length - 1];
                    string day = splitted[splitted.Length - 2];
                    formatted = formatted.Replace(time, "");
                    formatted = formatted.Replace(day, "");
                    string subject = formatted;
                }

            }
        }
    }

    private void addLectureToDay (Day day, string subject, string time)
    {
        day.addLecture(subject, time);
    }

    private void getJson()
    {

    }
}
