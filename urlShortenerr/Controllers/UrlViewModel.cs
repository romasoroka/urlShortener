﻿namespace urlShortenerr.Controllers
{
    public class UrlViewModel
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public string CreatedBy { get; set; }
        public bool CanEdit { get; set; }
    }
}