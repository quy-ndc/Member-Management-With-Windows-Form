﻿using BusinessObject;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess
{
    public class MemberDAO
    {
        // Get default user from appsettings
        private MemberObject GetDefaultMember()
        {
            MemberObject Default = null;
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
                string email = config["account:defaultAccount:email"];
                string password = config["account:defaultAccount:password"];
                Default = new MemberObject
                {
                    MemberID = 1,
                    Email = email,
                    Password = password,
                    City = "",
                    Country = "",
                    MemberName = "Admin"
                };
            }

            return Default;
        }

        // Initialize MemberList
        private static List<MemberObject> members = new List<MemberObject>
        {
            new MemberObject
            {
                MemberID = 2,
                MemberName = "QuyNDC",
                Email = "quyndc@gmail.com",
                Password = "1",
                City = "New York",
                Country = "United States"
            },
            new MemberObject
            {
                MemberID = 3,
                MemberName = "Khanh Ha",
                Email = "hahahaha@gmail.com",
                Password = "1",
                City = "Huế",
                Country = "Vietnam"
            },
            new MemberObject
            {
                MemberID = 4,
                MemberName = "Chung Quy",
                Email = "quyndcse173002@fpt.edu.vn",
                Password = "1",
                City = "Hollywood",
                Country = "United States"
            },
            new MemberObject
            {
                MemberID = 5,
                MemberName = "Ren Akiyama",
                Email = "knight@gmail.com",
                Password = "123456",
                City = "Tokyo",
                Country = "Japan"
            },
            new MemberObject
            {
                MemberID = 6,
                MemberName = "Elizabeth",
                Email = "eliza@gmail.com",
                Password = "123456",
                City = "London",
                Country = "United Kingdom"
            },
            new MemberObject
            {
                MemberID = 7,
                MemberName = "Kido Shinji",
                Email = "ryuki@gmail.com",
                Password = "123456",
                City = "Osaka",
                Country = "Japan"
            },
            new MemberObject
            {
                MemberID = 8,
                MemberName = "I ran out of idea for names",
                Email = "fake@gmail.com",
                Password = "123456",
                City = "Ho Chi Minh",
                Country = "Vietnam"
            },
            new MemberObject
            {
                MemberID = 9,
                MemberName = "Godai Yuusuke",
                Email = "kuuga@gmail.com",
                Password = "123456",
                City = "Tokyo",
                Country = "Japan"
            },
            new MemberObject {
                MemberID = 10,
                MemberName = "Tsugami Shouichi",
                Email = "agito@gmail.com",
                Password = "1",
                City = "Tokyo",
                Country = "Japan"
            },
            new MemberObject
            {
                MemberID = 11,
                MemberName = "Khanh Long",
                Email = "khlo@gmail.com",
                Password = "1",
                City = "Ho Chi Minh",
                Country = "Vietnam"
            }
        };
        private MemberDAO()
        {
            MemberObject DefaultAdmin = GetDefaultMember();
            if (DefaultAdmin != null)
            {
                members.Add(DefaultAdmin);
            }
        }

        // Using Singleton Pattern
        private static MemberDAO instance = null;
        private static object instanceLook = new object();

        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public List<MemberObject> GetMembersList => members;

        public MemberObject Login(string Email, string Password)
        {
            MemberObject member = members.SingleOrDefault(mb => mb.Email.Equals(Email) && mb.Password.Equals(Password));
            return member;
        }

        public MemberObject GetMember(int MemberId)
        {
            return members.SingleOrDefault(mb => mb.MemberID == MemberId);
        }
        public MemberObject GetMember(string MemberEmail)
        {
            return members.SingleOrDefault(mb => mb.Email.Equals(MemberEmail));
        }
        public void AddMember(MemberObject member)
        {
            if (member == null)
            {
                throw new Exception("Member is undefined!!");
            }

            if (GetMember(member.MemberID) == null && GetMember(member.Email) == null)
            {
                members.Add(member);
            }
            else
            {
                throw new Exception("Member is existed!!");
            }
        }
        public void Update(MemberObject member)
        {
            if (member == null)
            {
                throw new Exception("Member is undefined!!");
            }
            MemberObject mem = GetMember(member.MemberID);
            if (mem != null)
            {
                var index = members.IndexOf(mem);
                members[index] = member;
            }
            else
            {
                throw new Exception("Member does not exist!!");
            }
        }
        public void Delete(int MemberId)
        {
            MemberObject member = GetMember(MemberId);
            if (member != null)
            {
                members.Remove(member);
            }
            else
            {
                throw new Exception("Member does not exist!!");
            }
        }

        public IEnumerable<MemberObject> SearchMember(int id)
        {
            IEnumerable<MemberObject> searchResult = null;

            var memberSearch = from member in members
                               where member.MemberID == id
                               select member;
            searchResult = memberSearch;

            return searchResult;
        }
        public IEnumerable<MemberObject> SearchMember(string name)
        {
            IEnumerable<MemberObject> searchResult = null;

            var memberSearch = from member in members
                               where member.MemberName.ToLower().Contains(name.ToLower())
                               select member;
            searchResult = memberSearch;

            return searchResult;
        }

        public IEnumerable<MemberObject> FilterMemberByCountry(string country, IEnumerable<MemberObject> searchList)
        {
            IEnumerable<MemberObject> searchResult = null;

            var memberSearch = from member in searchList
                               where member.Country == country
                               select member;
            if (country.Equals("All"))
            {
                memberSearch = from member in searchList
                               select member;
            }
            searchResult = memberSearch;

            return searchResult;
        }

        public IEnumerable<MemberObject> FilterMemberByCity(string country, string city, IEnumerable<MemberObject> searchList)
        {
            IEnumerable<MemberObject> searchResult = null;

            var memberSearch = from member in searchList
                               where member.City == city
                               select member;
            if (city.Equals("All"))
            {
                memberSearch = from member in searchList
                               where member.Country == country
                               select member;
                if (country.Equals("All"))
                {
                    memberSearch = from member in searchList
                                   select member;
                }
            }
            searchResult = memberSearch;

            return searchResult;
        }
    }
}