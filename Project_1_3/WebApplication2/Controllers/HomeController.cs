using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using PagedList.Mvc;
using PagedList;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(int? page)
        {
            List<Item> list = (List<Item>)Session["list_With_Data"];
            if (list == null)
            {
                return View();
            }
            else
            {
                return View(list.ToPagedList((page ?? 1), 15));
            }
        }

        [HttpPost]
        public ActionResult Index(String list)
        {
            List<Item> list_of_Items = new List<Item>();
            Item it = new Item();
            string html = "http://belchip.by/search/?query=" + Request.Params["search"];
            HtmlDocument HD = new HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            HD = web.Load(html);
            HtmlNodeCollection NoAltElements = HD.DocumentNode.SelectNodes("//div[@class='cat-item']");
            if (NoAltElements != null)
            {
                HtmlNodeCollection text_Product = NoAltElements[1].SelectNodes("//h3");
                HtmlNodeCollection price_Of_Product = NoAltElements[4].SelectNodes("//div[@class='butt-add']");
                HtmlNodeCollection nod = NoAltElements.First().SelectNodes("//div[@class='cat-pic']");
                string itog = "";
                // Проверяем наличие узлов
                int i = 0;
                foreach (HtmlNode HN in nod)
                {
                    // Получаем строчки
                    Item item = new Item();
                    item.src = "http://belchip.by/" + HN.FirstChild.GetAttributeValue("href", "");
                    item.href = "http://belchip.by/" + HN.ChildNodes[1].GetAttributeValue("href", "");
                    item.name = text_Product[i + 1].InnerText;
                    item.price = price_Of_Product[i].FirstChild.InnerText;
                    if (item.price == "цена по запросу")
                        item.store = "No in storage";
                    else
                        item.store = "In storage";
                    list_of_Items.Add(item);
                    i++;
                }


                html = "https://www.ru-chipdip.by/search?searchtext=" + Request.Params["search"];
                html = html.Replace(" ", "+");
                HtmlDocument HD_Chip_Dip = new HtmlDocument();
                var web_Chip_Dip = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                HD_Chip_Dip = web_Chip_Dip.Load(html);
                List<string> links_Items = it.get_Links(HD_Chip_Dip);
                foreach (string temp in links_Items)
                {
                    HD_Chip_Dip = web.Load(temp);
                    while (true)
                    {
                        foreach (HtmlNode chip_Item in HD_Chip_Dip.DocumentNode.SelectNodes("//tr[@class='with-hover']"))
                        {
                            Item item = new Item();
                            item.href = "https://www.ru-chipdip.by" + chip_Item.ChildNodes[1].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "");
                            item.name = chip_Item.ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;
                            item.src = chip_Item.SelectSingleNode(".//img[@class='img75']") == null ? "Content/404.jpg" : chip_Item.SelectSingleNode(".//img[@class='img75']").Attributes["src"].Value;
                            item.price = chip_Item.SelectSingleNode(".//span[contains(@class, 'price_mr')]").InnerText;
                            item.store = chip_Item.SelectSingleNode(".//span[@class='item__avail_available']") == null ? "No in storage" : "In storage";
                            list_of_Items.Add(item);
                        }

                        HtmlNode next_Page = HD_Chip_Dip.DocumentNode.SelectSingleNode("//a[@class='link no-visited pager__control pager__next']");
                        if (next_Page == null)
                        {
                            break;
                        }
                        else
                        {
                            HD_Chip_Dip = web.Load("https://www.ru-chipdip.by" + next_Page.GetAttributeValue("href", "").Replace("&amp;", "&"));
                        }
                    }
                }

            }
            Session["list_With_Data"] = list_of_Items;
            return View(list_of_Items.ToPagedList(1, 15));
        }
    }
}