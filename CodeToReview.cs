using System;
using System.Collegctions.Generic;
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People
    {
        //Jigin: Use DateTimeOffset consistently throughout to avoid mixing DateTime and DateTimeOffset,
        // which can cause ambiguous conversions and timezone-related bugs.
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
     public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public People(string name) : this(name, Under16.Date) { }
     public People(string name, DateTime dob) {
         Name = name;
         DOB = dob;
     }}

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people;

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param>
        /// <returns>List<object></returns>

        // Jigin: Results accumulate across calls because _people is instance-level state.
        // e.g. GetPeople(2) then GetPeople(3) returns 5 entries, not 3.
        // To reset between calls, move _people declaration inside this method.
        public List<People> GetPeople(int i)
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name
                    string name = string.Empty;
                    //Jigin : random can be moved outside the for loop
                    var random = new Random();
                    if (random.Next(0, 1) == 0) {
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    // Jigin: Not sure, but why are we using 356? Is it 365? Also, it can be declared as a constant and reused in GetBobs
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                //Jigin: e is declare but never used, better to just throw without exception, it will preserve the original stack
                //Jigin: Also better to log error somewhere before you throw exception
                catch (Exception e)
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation");
                }
            }
            return _people;
        }

        //Any particular reason this method is private , could not see any usage in this class file
        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            //Jigin: why is the Name hardcoded? Ideally the the function name should be GetPeopleByName with input parameter of string peopleName and passed instead of Bob
            // Jigin: better to use x.DOB <= DateTimeOffset.UtcNow.AddYears(-30) , current logic for Younger than 30 
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }

        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test"))
                return p.Name;

            //Jigin : .Length will be int and lastName is string - also have lastName.Length
            if ((p.Name.Length + lastName).Length > 255)
            {
                (p.Name + " " + lastName).Substring(0, 255);
            }

            //Jigin: repeated inside the above if block , could be assigned to a variable and reused here
            return p.Name + " " + lastName;
        }
    }
}