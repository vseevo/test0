using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using test_we.Models;

namespace test_we.Controllers
{
    [Route("api")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost("sum")]
        public List<SumStruct> Sum([FromBody]SumStruct[] dtos)
        {
            List<SumStruct> resp = new List<SumStruct>();
            int found;
            for(int i = 0; i < dtos.Length; i++)
            {
                found = resp.IndexOf(dtos[i]);
                if(found > -1)
                {
                    resp[found].sum(dtos[i]);
                }
                else
                {
                    resp.Add(dtos[i]);
                }
            }
            BinaryWriter w = new BinaryWriter(System.IO.File.Open("sum.dat", FileMode.OpenOrCreate));
                for(int i = 0; i < resp.Count; i++)
                {
                w.Write(i);
                w.Write(resp[i].GetAddress());
                w.Write(resp[i].GetCash());
                w.Write(resp[i].GetMoney());
                w.Write(resp[i].GetPrice());
                }
            w.Close();
            return resp;
        }
        [HttpGet("sum")]
        public List<SumStruct> Sum()
        {
            string json = "";
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create("https://collector.weekendagency.ru/api/account/test");
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    json += sLine;
            }
            SumStruct[] result = JsonConvert.DeserializeObject<SumStruct[]>(json);
            return Sum(result);
        }
        [HttpGet("get")]
        public string Get()
        {
            string ret = "";
            using (BinaryReader r = new BinaryReader(System.IO.File.Open("sum.dat", FileMode.Open)))
            {
                while (r.PeekChar() > -1)
                {
                    int id = r.ReadInt32();
                    string address = r.ReadString();
                    string cash = r.ReadString();
                    int money = r.ReadInt32();
                    int price = r.ReadInt32();

                    ret+=String.Format("id: {0}  address: {1}  cash: {2} money: {3} price: {4} ",
                        id, address, cash, money, price);
                }
            }
            return ret;
        }
    }
}
