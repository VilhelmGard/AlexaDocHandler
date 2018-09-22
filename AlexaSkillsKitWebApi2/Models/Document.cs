using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexaSkillsKitWebApi2.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public Document(int id, string name, string content)
        {
            Id = id;
            Name = name;
            Content = content;
        }
    }
}