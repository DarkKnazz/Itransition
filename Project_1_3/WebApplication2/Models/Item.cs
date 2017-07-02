using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Item
    {
        public string src;
        public string href;
        public string name;
        public string price;
        public string store;
        public List<string> get_Links(HtmlDocument HD){
            HtmlDocument html = HD;
            List<string> temp_List = new List<string>();
            while(true){
                HtmlNodeCollection page_Links = html.DocumentNode.SelectNodes("//td[@class='group-header-wrap']/a");
                foreach(HtmlNode item in page_Links){
                    string temp = "https://www.ru-chipdip.by" + item.GetAttributeValue("href", "").Replace("catalog-show","catalog");
                    temp_List.Add(temp);
                }

                HtmlNode next_Page = html.DocumentNode.SelectSingleNode("//a[@class='link no-visited pager__control pager__next']");
                if(next_Page == null){
                    break;
                }
                else{
                    HtmlWeb new_Web = new HtmlWeb();
                    html = new_Web.Load("https://www.ru-chipdip.by" + next_Page.GetAttributeValue("href", "").Replace("&amp;", "&"));
                }
            }

            return temp_List;
        }
    }
}