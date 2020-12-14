﻿using System;

namespace RVT.Common.Messages
{
    public class ChooserLbMessage
    {
        public string IDNP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime? Birth_date { get; set; }
        public DateTime Vote_date { get; set; }
        public int Region { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// IDVN ID unic numeric a votantului
        /// </summary>
        /// 
        public string IDVN { get; set; }
        public int PartyChoosed { get; set; }
    }
}
