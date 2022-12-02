using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBox.Models
{
    public class Attachment
    {
        public Payload payload { get; set; }
        public string type { get; set; }
    }

    public class Element
    {
        public string image_url { get; set; }
        public string subtitle { get; set; }
        public string title { get; set; }
    }

    public class Message
    {
        public Attachment attachment { get; set; }
        public string text { get; set; }
    }

    public class Payload
    {
        public List<Element> elements { get; set; }
        public string template_type { get; set; }
    }

    public class Recipient
    {
        public string user_id { get; set; }
    }

    public class RequestSendInfo
    {
        public Recipient recipient { get; set; }
        public Message message { get; set; }
    }


}