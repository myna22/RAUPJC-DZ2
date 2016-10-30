using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad3_4_5
{
    class Program
    {
        static void Main(string[] args)
        {
            // 3.zad
            int[] integers = new[] { 1, 2, 2, 2, 3, 3, 4, 5 };
            string[] strings = integers.GroupBy(i => i).Select(n => "Broj" + n.Key + "se ponavlja" + n.Count() + "puta").ToArray();

            //4.zad
            Example1();
            Example2();

            //5.zad
            University[] universities = GetAllCroatianUniversities();
            Student[] allCroatianStudents = universities.SelectMany(s => s.Students).ToArray();
            Student[] croatianStudentsOnMultipleUniversities = universities.SelectMany(s => s.Students).GroupBy(s => s).Where(s => s.Count() > 1).Select(s => s.First()).ToArray();
            Student[] studentsOnMaleOnlyUniversities = universities.Where(uni => uni.Students.Where(s => s.Gender == Gender.Female).Count() == 0).SelectMany(uni => uni.Students).Distinct().ToArray();
        }

        public class Student
        { 
            public string Name { get; set; }
            public string Jmbag { get; set; }
            public Gender Gender { get; set; }
            public Student(string name, string jmbag)
            {
                Name = name;
                Jmbag = jmbag;
            }
            public Student(string name, string jmbag, Gender gender)
            {
                Name = name;
                Jmbag = jmbag;
                Gender = gender;
            }

            public override bool Equals(object obj)
            {
                var item = obj as Student;

                if (item == null)
                {
                    return false;
                }

                return Jmbag.Equals(item.Jmbag) && Name.Equals(item.Name) && Gender.Equals(item.Gender);
            }

            public override int GetHashCode()
            {
                return Jmbag.GetHashCode();
            }

            public static bool operator ==(Student a, Student b)
            {
                // If both are null, or both are same instance, return true.
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return a.Jmbag == b.Jmbag && a.Gender == b.Gender && a.Name == b.Name;
            }

            public static bool operator !=(Student a, Student b)
            {
                return !(a == b);
            }
        }
    
        public enum Gender
        {
            Male, Female
        }

        static void Example1()
        {
            var list = new List<Student>()
            {
             new  Student("Ivan", jmbag:"001234567")
            };
            var ivan = new Student("Ivan", jmbag: "001234567");
            // false :(
            bool anyIvanExists = list.Any(s => s==ivan);
            Console.WriteLine(anyIvanExists);
        }
        static void Example2()
        {
            var list = new List<Student>()
            {
             new  Student("Ivan", jmbag:"001234567"),
            new  Student("Ivan", jmbag:"001234567")
            };
            // 2 :(
            var distinctStudents = list.Distinct().Count();
        }

        //5.zad
        public class University
        {
            public string Name { get; set; }
            public Student[] Students { get; set; }
            
            public University(string name, Student[] students)
            {
                Name = name;
                Students = students;
            }
        }

        private static University[] GetAllCroatianUniversities()
        {
            var list = new List<University>()
            {
                new University("FER", students: FER()),
                new University("Agronomski fakultet", students: AgronomskiFakultet()),
                new University("Građevinski fakultet", students: GrađevinskiFakultet()),
            };
            return list.ToArray();
        }

        static Student[] FER()
        {
            var list = new List<Student>()
            {
                new Student ("Andrej", jmbag: "0036469495", gender: Gender.Male),
                new Student ("Mihael", jmbag: "0037468888", gender: Gender.Male),
                new Student ("Marin", jmbag: "00364546234", gender: Gender.Male)
            };
            return list.ToArray();
        }

        static Student[] AgronomskiFakultet()
        {
            var list = new List<Student>()
            {
                new Student ("Maja", jmbag: "003589653", gender: Gender.Female),
                new Student ("Ivana", jmbag: "004568712", gender: Gender.Female),
                new Student ("Benjamin", jmbag: "003648954", gender: Gender.Male),
                new Student ("Marin", jmbag: "003645213", gender: Gender.Male)
            };
            return list.ToArray();
        }

        static Student[] GrađevinskiFakultet()
        {
            var list = new List<Student>()
            {
                new Student ("Ivan", jmbag: "004668812", gender: Gender.Male),
                new Student ("Mihael", jmbag: "003646689", gender: Gender.Male),
                new Student ("Maja", jmbag: "003589653", gender: Gender.Female)
            };
            return list.ToArray();
        }
    }
}
