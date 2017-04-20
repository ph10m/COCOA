using System;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using COCOA.Models;

/// <summary>
/// Webscraper grabs schedule information and keeps
/// each lecture in a "Lecture"-object.
/// Takes "username" from 1024 in the constructor.
/// </summary>
public class WikiSearch
{
    private string search, searchUrl;

    public WikiSearch(string search)
    {
        this.searchUrl = "https://www.wikipendium.no/search/?q=";
    
        var html = new HtmlDocument();
        html.LoadHtml(new WebClient().DownloadString(searchUrl));
        var root = html.DocumentNode;
        Console.WriteLine(root);
        Console.WriteLine(search);
        Console.WriteLine(html);
        // select nodes under table id=lecture and under the tag tbody
        foreach (HtmlNode node in root.SelectNodes("//*[@id='suggestions']"))
        {
            Console.WriteLine(node.InnerHtml);
            foreach (var child in node.ChildNodes)
            {
                Console.WriteLine(child.InnerText);
            }
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
}