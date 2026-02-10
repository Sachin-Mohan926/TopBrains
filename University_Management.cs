

using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    public class Course
    {
        public string CourseCode { get; private set; }
        public string CourseName { get; private set; }
        public int Credits { get; private set; }
        public int MaxCapacity { get; private set; }
        public int CurrentEnrollment { get; private set; }

        // Store prerequisites as course codes (simpler + matches spec)
        public List<string> PrerequisiteCodes { get; private set; }

        public Course(string code, string name, int credits, int maxCapacity, List<string>? prerequisiteCodes = null)
        {
            CourseCode = code;
            CourseName = name;
            Credits = credits;
            MaxCapacity = maxCapacity;
            CurrentEnrollment = 0;
            PrerequisiteCodes = prerequisiteCodes ?? new List<string>();
        }

        public bool IsFull() => CurrentEnrollment >= MaxCapacity;

        public bool HasPrerequisites(List<string> completedCourses)
        {
            // If no prerequisites, you're good
            if (PrerequisiteCodes == null || PrerequisiteCodes.Count == 0) return true;

            // Must contain ALL prerequisite codes
            return PrerequisiteCodes.All(p => completedCourses.Contains(p, StringComparer.OrdinalIgnoreCase));
        }

        public void EnrollStudent()
        {
            if (IsFull())
                throw new InvalidOperationException("Course is full.");

            CurrentEnrollment++;
        }

        public void DropStudent()
        {
            if (CurrentEnrollment > 0)
                CurrentEnrollment--;
        }
    }

    public class Student
    {
        public string StudentId { get; private set; }
        public string Name { get; private set; }
        public string Major { get; private set; }
        public int MaxCredits { get; private set; }
        public List<string> CompletedCourses { get; private set; }
        public List<Course> RegisteredCourses { get; private set; }

        public Student(string id, string name, string major, int maxCredits = 18, List<string>? completedCourses = null)
        {
            StudentId = id;
            Name = name;
            Major = major;
            MaxCredits = maxCredits;

            CompletedCourses = completedCourses ?? new List<string>();
            RegisteredCourses = new List<Course>();
        }

        public int GetTotalCredits() => RegisteredCourses.Sum(c => c.Credits);

        public bool CanAddCourse(Course course, out string reason)
        {
            reason = "";

            if (RegisteredCourses.Any(c => c.CourseCode.Equals(course.CourseCode, StringComparison.OrdinalIgnoreCase)))
            {
                reason = "Student is already registered for this course.";
                return false;
            }

            if (GetTotalCredits() + course.Credits > MaxCredits)
            {
                reason = $"Credit limit exceeded. Current: {GetTotalCredits()}/{MaxCredits}, Course adds: {course.Credits}.";
                return false;
            }

            if (!course.HasPrerequisites(CompletedCourses))
            {
                reason = "Prerequisites not met.";
                return false;
            }

            return true;
        }

        public bool AddCourse(Course course, out string reason)
        {
            reason = "";

            if (course.IsFull())
            {
                reason = "Course is full.";
                return false;
            }

            if (!CanAddCourse(course, out reason))
                return false;

            RegisteredCourses.Add(course);
            course.EnrollStudent();
            return true;
        }

        public bool DropCourse(string courseCode, out string reason)
        {
            reason = "";
            var course = RegisteredCourses.FirstOrDefault(c => c.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase));

            if (course == null)
            {
                reason = "Student is not registered in this course.";
                return false;
            }

            RegisteredCourses.Remove(course);
            course.DropStudent();
            return true;
        }

        public void DisplaySchedule()
        {
            Console.WriteLine($"\nSchedule for {Name} (ID: {StudentId})");
            Console.WriteLine(new string('-', 55));

            if (RegisteredCourses.Count == 0)
            {
                Console.WriteLine("No registered courses.");
                return;
            }

            Console.WriteLine($"{ "Code",-10}{ "Name",-30}{ "Credits",8}");
            Console.WriteLine(new string('-', 55));

            foreach (var c in RegisteredCourses)
                Console.WriteLine($"{ c.CourseCode,-10}{ Truncate(c.CourseName, 30),-30}{ c.Credits,8}");

            Console.WriteLine(new string('-', 55));
            Console.WriteLine($"Total Credits: {GetTotalCredits()}/{MaxCredits}");
        }

        private static string Truncate(string s, int len) => s.Length <= len ? s : s.Substring(0, len - 3) + "...";
    }

    public class UniversitySystem
    {
        public Dictionary<string, Course> AvailableCourses { get; private set; } = new Dictionary<string, Course>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Student> Students { get; private set; } = new Dictionary<string, Student>(StringComparer.OrdinalIgnoreCase);
        public List<Student> ActiveStudents { get; private set; } = new List<Student>();

        public void AddCourse(string code, string name, int credits, int maxCapacity = 50, List<string>? prerequisites = null)
        {
            ValidateCodeOrId(code, "Course code");

            if (AvailableCourses.ContainsKey(code))
                throw new ArgumentException("Course code already exists.");

            if (credits < 1 || credits > 4)
                throw new ArgumentException("Credits must be between 1 and 4.");

            if (maxCapacity < 10 || maxCapacity > 100)
                throw new ArgumentException("Capacity must be between 10 and 100.");

            prerequisites ??= new List<string>();

            // Cannot include itself
            if (prerequisites.Any(p => p.Equals(code, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Prerequisites cannot include the course itself.");

            AvailableCourses[code] = new Course(code, name, credits, maxCapacity, prerequisites);
        }

        public void AddStudent(string id, string name, string major, int maxCredits = 18, List<string>? completedCourses = null)
        {
            ValidateCodeOrId(id, "Student ID");

            if (Students.ContainsKey(id))
                throw new ArgumentException("Student ID already exists.");

            if (maxCredits > 24)
                throw new ArgumentException("Max credits cannot exceed 24.");

            var student = new Student(id, name, major, maxCredits, completedCourses);
            Students[id] = student;
            ActiveStudents.Add(student);
        }

        public bool RegisterStudentForCourse(string studentId, string courseCode)
        {
            if (!Students.TryGetValue(studentId, out var student))
            {
                Console.WriteLine("Student not found.");
                return false;
            }

            if (!AvailableCourses.TryGetValue(courseCode, out var course))
            {
                Console.WriteLine("Course not found.");
                return false;
            }

            if (student.AddCourse(course, out var reason))
            {
                Console.WriteLine($"Registration successful! Total credits: {student.GetTotalCredits()}/{student.MaxCredits}.");
                return true;
            }

            Console.WriteLine($"Registration failed: {reason}");
            return false;
        }

        public bool DropStudentFromCourse(string studentId, string courseCode)
        {
            if (!Students.TryGetValue(studentId, out var student))
            {
                Console.WriteLine("Student not found.");
                return false;
            }

            if (student.DropCourse(courseCode, out var reason))
            {
                Console.WriteLine("Course dropped successfully.");
                return true;
            }

            Console.WriteLine($"Drop failed: {reason}");
            return false;
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("\nAvailable Courses");
            Console.WriteLine(new string('-', 90));

            if (AvailableCourses.Count == 0)
            {
                Console.WriteLine("No courses available.");
                return;
            }

            Console.WriteLine($"{ "Code",-10}{ "Name",-35}{ "Cr",4}{ "Enroll",10}{ "Capacity",10}{ "Prerequisites",-20}");
            Console.WriteLine(new string('-', 90));

            foreach (var c in AvailableCourses.Values.OrderBy(c => c.CourseCode))
            {
                string prereq = (c.PrerequisiteCodes.Count == 0) ? "-" : string.Join(",", c.PrerequisiteCodes);
                Console.WriteLine($"{ c.CourseCode,-10}{ Truncate(c.CourseName,35),-35}{ c.Credits,4}{ c.CurrentEnrollment,10}{ c.MaxCapacity,10}{ Truncate(prereq,20),-20}");
            }

            Console.WriteLine(new string('-', 90));
        }

        public void DisplayStudentSchedule(string studentId)
        {
            if (!Students.TryGetValue(studentId, out var student))
            {
                Console.WriteLine("Student not found.");
                return;
            }

            student.DisplaySchedule();
        }

        public void DisplaySystemSummary()
        {
            int totalStudents = Students.Count;
            int totalCourses = AvailableCourses.Count;
            double averageEnrollment = totalCourses > 0 ? AvailableCourses.Values.Average(c => c.CurrentEnrollment) : 0;

            Console.WriteLine("\nUniversity System Summary");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Total Students: {totalStudents}");
            Console.WriteLine($"Total Courses : {totalCourses}");
            Console.WriteLine($"Avg Enrollment per Course: {averageEnrollment:F2}");
        }

        private static void ValidateCodeOrId(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} cannot be empty.");

            value = value.Trim();

            // 3-10 alphanumeric
            if (value.Length < 3 || value.Length > 10 || !value.All(char.IsLetterOrDigit))
                throw new ArgumentException($"{fieldName} must be 3-10 alphanumeric characters.");
        }

        private static string Truncate(string s, int len) => s.Length <= len ? s : s.Substring(0, len - 3) + "...";
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var universitySystem = new UniversitySystem();

            // Optional: prepopulate for demo
            try
            {
                universitySystem.AddCourse("MATH100", "Basic Mathematics", 3, 30);
                universitySystem.AddCourse("CS101", "Introduction to Programming", 3, 30, new List<string> { "MATH100" });
                universitySystem.AddStudent("S001", "Alice Johnson", "Computer Science", 18, new List<string> { "MATH100" });
            }
            catch { /* ignore demo errors */ }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n===== University Management System =====");
                Console.WriteLine("1. Add a Course");
                Console.WriteLine("2. Add a Student");
                Console.WriteLine("3. Register Student for Course");
                Console.WriteLine("4. Drop Student from Course");
                Console.WriteLine("5. Display All Courses");
                Console.WriteLine("6. Display Student Schedule");
                Console.WriteLine("7. Display System Summary");
                Console.WriteLine("8. Exit");
                Console.Write("Select an option (1-8): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCourseUI(universitySystem);
                        break;

                    case "2":
                        AddStudentUI(universitySystem);
                        break;

                    case "3":
                        RegisterUI(universitySystem);
                        break;

                    case "4":
                        DropUI(universitySystem);
                        break;

                    case "5":
                        universitySystem.DisplayAllCourses();
                        break;

                    case "6":
                        DisplayScheduleUI(universitySystem);
                        break;

                    case "7":
                        universitySystem.DisplaySystemSummary();
                        break;

                    case "8":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void AddCourseUI(UniversitySystem system)
        {
            try
            {
                Console.Write("Enter Course Code (3-10 alphanumeric): ");
                string code = (Console.ReadLine() ?? "").Trim();

                Console.Write("Enter Course Name: ");
                string name = (Console.ReadLine() ?? "").Trim();

                int credits = ReadInt("Enter Credits (1-4): ", 1, 4);

                Console.Write("Enter Max Capacity (10-100) [default 50]: ");
                string capText = (Console.ReadLine() ?? "").Trim();
                int capacity = 50;
                if (!string.IsNullOrEmpty(capText))
                {
                    if (!int.TryParse(capText, out capacity))
                        throw new ArgumentException("Capacity must be a number.");
                    if (capacity < 10 || capacity > 100)
                        throw new ArgumentException("Capacity must be between 10 and 100.");
                }

                Console.Write("Enter Prerequisites (comma-separated codes, or Enter for none): ");
                string prereqText = (Console.ReadLine() ?? "").Trim();
                List<string> prereqs = ParseCsv(prereqText);

                system.AddCourse(code, name, credits, capacity, prereqs);
                Console.WriteLine($"Course {code} added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding course: {ex.Message}");
            }
        }

        private static void AddStudentUI(UniversitySystem system)
        {
            try
            {
                Console.Write("Enter Student ID (3-10 alphanumeric): ");
                string id = (Console.ReadLine() ?? "").Trim();

                Console.Write("Enter Name: ");
                string name = (Console.ReadLine() ?? "").Trim();

                Console.Write("Enter Major: ");
                string major = (Console.ReadLine() ?? "").Trim();

                Console.Write("Enter Max Credits (<=24) [default 18]: ");
                string maxText = (Console.ReadLine() ?? "").Trim();
                int maxCredits = 18;
                if (!string.IsNullOrEmpty(maxText))
                {
                    if (!int.TryParse(maxText, out maxCredits))
                        throw new ArgumentException("Max credits must be a number.");
                    if (maxCredits > 24)
                        throw new ArgumentException("Max credits cannot exceed 24.");
                    if (maxCredits < 1)
                        throw new ArgumentException("Max credits must be at least 1.");
                }

                Console.Write("Enter Completed Courses (comma-separated, or Enter for none): ");
                string completedText = (Console.ReadLine() ?? "").Trim();
                List<string> completed = ParseCsv(completedText);

                system.AddStudent(id, name, major, maxCredits, completed);
                Console.WriteLine($"Student {id} added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student: {ex.Message}");
            }
        }

        private static void RegisterUI(UniversitySystem system)
        {
            Console.Write("Enter Student ID: ");
            string studentId = (Console.ReadLine() ?? "").Trim();

            Console.Write("Enter Course Code: ");
            string courseCode = (Console.ReadLine() ?? "").Trim();

            system.RegisterStudentForCourse(studentId, courseCode);
        }

        private static void DropUI(UniversitySystem system)
        {
            Console.Write("Enter Student ID: ");
            string studentId = (Console.ReadLine() ?? "").Trim();

            Console.Write("Enter Course Code: ");
            string courseCode = (Console.ReadLine() ?? "").Trim();

            system.DropStudentFromCourse(studentId, courseCode);
        }

        private static void DisplayScheduleUI(UniversitySystem system)
        {
            Console.Write("Enter Student ID: ");
            string studentId = (Console.ReadLine() ?? "").Trim();

            system.DisplayStudentSchedule(studentId);
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();

                if (!int.TryParse(input, out int value))
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"Value must be between {min} and {max}. Try again.");
                    continue;
                }

                return value;
            }
        }

        private static List<string> ParseCsv(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();

            return text
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}