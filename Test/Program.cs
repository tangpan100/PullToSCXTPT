using System;
using System.Collections.Generic;

namespace PullToScxtpt.Test
{

    class Program
    {

        //static void Main(string[] args)
        //{

        //    var jack = new Person() { Name = "Jack", Age = 32, Gender = "male" };

        //    var lily = new Person() { Name = "Lily", Age = 23, Gender = "female" };

        //    var park = new Person() { Name = "Park", Age = 52, Gender = "male" };

        //    var ted = new Person() { Name = "Ted", Age = 13, Gender = "male", Remark = "his is child" };

        //    List<Person> group1 = new List<Person>() { jack, lily, ted };

        //    //park.Group = group1;

        //    //jack.Group = group1;

        //    //lily.Group = group1;

        //    //ted.Group = group1;



        //    JsonSerializerSettings jsSetting = new JsonSerializerSettings();

        //    jsSetting.NullValueHandling = NullValueHandling.Ignore;

        //    string jackJson = JsonConvert.SerializeObject(jack, Formatting.Indented, jsSetting);

        //    string tedJson = JsonConvert.SerializeObject(ted, Formatting.Indented, jsSetting);

        //    string groupJson = JsonConvert.SerializeObject(group1);

        //    string parkJson = JsonConvert.SerializeObject(park, Formatting.Indented, jsSetting);

        //    Console.WriteLine(jackJson);

        //    Console.WriteLine(tedJson);

        //    Console.WriteLine(parkJson);

        //    string jsonTextColl = jackJson + tedJson + parkJson;

        //    Person result1 = JsonConvert.DeserializeObject(jackJson) as Person;

        //    Person result2 = JsonConvert.DeserializeObject<Person>(jackJson);

        //    List<Person> result3 = JsonConvert.DeserializeObject<List<Person>>(groupJson);

        //    List<Person> result4 = JsonConvert.DeserializeObject(groupJson) as List<Person>;

        //  //  List<Person> result5 = JsonConvert.DeserializeObject(jsonTextColl) as List<Person>;

        //    Console.ReadKey();

        //}

        static void Main(string[] args)
        {
            string privateKey = System.Configuration.ConfigurationManager.AppSettings["privateKey"].ToString();
            string sign = RSAHelper.RSASignPEM("12346", privateKey, "SHA-1withRSA", "UTF-8");
        }
    
    }



    class Person
    {

        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string Remark { get; set; }

        //public List<Person> Group { get; set; }

    }

}