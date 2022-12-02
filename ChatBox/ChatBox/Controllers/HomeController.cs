using ChatBox.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChatBox.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using(Connection connection =new Connection())
            {
            return View(connection.ZaloUsers.ToList());

            }
        }

        public async Task<JsonResult> ZaloFollowers(FormCollection collection)
        {
            JsonResult jr = new JsonResult();
            string token = collection["token"];
            string offset = collection["offset"];
            string count = collection["count"];

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("access_token", token);
            client.BaseAddress = new Uri("https://openapi.zalo.me/v2.0/oa/");
            List<Follower> listUser = new List<Follower>();
            JavaScriptSerializer ser = new JavaScriptSerializer();


            var listfollowers = await client.GetAsync("getfollowers?data={\"offset\":" + offset + ",\"count\":" + count + "}");
            var content = await listfollowers.Content.ReadAsStringAsync();

            Dictionary<string, object> json = ser.Deserialize<Dictionary<string, object>>(content);
            Dictionary<string, object> data = (Dictionary<string, object>)json["data"];
            ArrayList followers = (ArrayList)data["followers"];
            foreach (Dictionary<string, object> item in followers)
            {
                string user_id = item["user_id"].ToString();

                Follower fl = new Follower();
                fl.user_id = user_id;
                listUser.Add(fl);
            }
            var clientInfo = new HttpClient();
            clientInfo.DefaultRequestHeaders.Add("access_token", token);
            clientInfo.BaseAddress = new Uri("https://openapi.zalo.me/v2.0/oa/getprofile");
            using (Connection connection = new Connection())
            {
                foreach (Follower id in listUser)
                {
                    var info = await client.GetAsync("getprofile?data={\"user_id\":" + id.user_id + "}");
                    var contentInfo = await info.Content.ReadAsStringAsync();

                    Dictionary<string, object> jsonInfo = ser.Deserialize<Dictionary<string, object>>(contentInfo);
                    Dictionary<string, object> dataInfo = (Dictionary<string, object>)jsonInfo["data"];
                    Dictionary<string, object> sharedinfo = (Dictionary<string, object>)dataInfo["shared_info"];

                    String phone = (String)sharedinfo["phone"];

                    ZaloUser zaloUser = new ZaloUser();
                    zaloUser.User_ID = (String)dataInfo["user_id"];
                    zaloUser.Avatar = (String)dataInfo["avatar"]; ;
                    zaloUser.Name = (String)dataInfo["display_name"];
                    zaloUser.Gender = (int)dataInfo["user_gender"];
                    connection.ZaloUsers.Add(zaloUser);
                    connection.SaveChanges();


                }
                jr.Data = new
                {
                    status = "OK",
                    message = "Hoàn Thành"
                };
                return Json(jr, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> RequestUpdate()
        {
            List<ZaloUser> zaloUsers = new List<ZaloUser>();        
            using(Connection connection=new Connection())
            {
                var user = connection.ZaloUsers.ToList();
                zaloUsers.AddRange(user);
            }
            foreach(ZaloUser item in zaloUsers)
            {
                var jsonData = "{\"recipient\": {\"user_id\": " + item.User_ID + "},\"message\": {\"attachment\": {\"payload\": {\"elements\": [{\"image_url\": \"https://developers.zalo.me/web/static/zalo.png\",\"subtitle\": \"Đang yêu cầu thông tin từ bạn\",\"title\": \"NguuyenDuyStore\"}],\"template_type\": \"request_user_info\"},\"type\": \"template\"},\"text\": \"Xin chào! Chúc ngày mới tốt lành\"}}";

                var js = System.Text.Json.JsonSerializer.Serialize(jsonData);
                var requestContent = new StringContent(js, Encoding.UTF8, "application/json");




                string token = "j9Ib66__xaoqbQ0URl3gJRYVfnvixAmF_ls1FcNfm36nsFKx6B7K4AsucmevbBKOWQYA6GwTd4ovbwzeBfoVEhoNhJy6ZuqpWUUv7W7gf4cKyvXyHzQJKE_3kM52gVPoaxN1V4dRpKRurVDCRDxT7SF0apSPmh8nYDhz5mVeop-ZrFbaABdlNlkYsNrxnjTIrFtfNttQ_adprTGJ8EMQ9e20fm9RaQnMq9dXNKI6pmdhhky_Svt92_kt-0XDWDGvbgpe3pgwrmE8tiacBUhIAABbdNiQyDrJgipSGX-az1YgeBuSFxQBIQkyb5Wlj9jhfz61PHtdZq2mZyPbubrvOcBdw4a";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("access_token", token);

                String url = "https://openapi.zalo.me/v2.0/oa/message";
                var response = await client.PostAsync(url, requestContent);
                var content = await response.Content.ReadAsStringAsync();
                //var json = JsonConvert.DeserializeObject<StringContent>(content);
            }

            return View();
        }

        

    }
}

