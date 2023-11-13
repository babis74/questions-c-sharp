using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Question
{
    public string Text { get; set; }
    public List<string> Answers { get; set; }

    public Question(string text, List<string> answers)
    {
        Text = text;
        Answers = answers;
    }
}

class Program
{
    static void Main()
    {
        List<Question> questions = new List<Question>
        {
            new Question("Ποιό είναι το αγαπημένο σου χρώμα;", new List<string> { "Κόκκινο", "Μπλέ", "Πράσινο" }),
            new Question("What is your preferred pet?", new List<string> { "Σκύλος", "Γάτα", "Χάμστερ" })
            // Add more questions and their respective answers here
        };

        List<Dictionary<Question, string>> allUserResponses = new List<Dictionary<Question, string>>();

        bool moreUsers = true;

        while (moreUsers)
        {
            Dictionary<Question, string> userResponses = new Dictionary<Question, string>();

            foreach (Question q in questions)
            {
                Console.WriteLine(q.Text);
                for (int i = 0; i < q.Answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {q.Answers[i]}");
                }

                Console.Write("Ή επιλογή σου (διάλεξε απάντηση): ");
                int userChoice;

                while (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < 1 || userChoice > q.Answers.Count)
                {
                    Console.WriteLine("Λάθος επιλογή!. Διάλεξε ένα αριθμό απο 1-3.");
                    Console.Write("Ή επιλογή σου: ");
                }

                userResponses[q] = q.Answers[userChoice - 1];
            }

            allUserResponses.Add(userResponses);

            Console.Write("Θέλεις να συνεχίσεις με επόμενη καταγραφη ερωτηματολογίου? (yes/no): ");
            string continueInput = Console.ReadLine().ToLower();

            if (continueInput != "yes")
            {
                moreUsers = false;
            }
        }

        string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(appFolderPath, "survey_results.txt");

        Console.WriteLine("Αποτελέσματα:");

        using (StreamWriter file = new StreamWriter(filePath))
        {
            foreach (Question q in questions)
            {
                file.WriteLine(q.Text);
                foreach (string answer in q.Answers)
                {
                    double count = allUserResponses.SelectMany(r => r.Where(kv => kv.Key == q && kv.Value == answer)).Count();
                    double percentage = (count / allUserResponses.Count) * 100;
                    file.WriteLine($"{answer}: {percentage}%");
                }
                file.WriteLine();
            }
        }

        Console.WriteLine($"Τα αποτελέσματα αποθηκεύτηκαν εδώ {filePath}");
    }
}




