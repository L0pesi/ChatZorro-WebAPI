using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatZorro.Entities
{
    internal class Message : Services.Configs
    {
        internal Message()
        {
            base.Load();
        }

        public int Code { get; set; }
        public int FromCode { get; set; }
        [Required]
        public int ChatCode { get; set; }
        public Body Body { get; set; }
        [Required]
        public InfoChat Chat { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }

    internal class Body
    {
        public Message Response { get; set; }
        [Required]
        public string Text { get; set; }
        public List<Attachment> Attachment { get; set; }
    }

    internal class Attachment
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }

    internal class Messg
    {
        public int id { get; set; }
        public int sender { get; set; }
        public string body { get; set; }
        public DateTime timer { get; set; }
        public int status { get; set; }
        public int recvId { get; set; }
        public bool recvIsGroup { get; set; }
    }
}
