using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;
using _200_OK.Data.Entity;
using System.Runtime.Remoting.Contexts;


namespace _200_OK
{
    [Synchronization]
    static class Program
    {
        public delegate string SelectChoice();
        public static Operation _operation = null; // This is Operation Class for Getting Data from Facebook.
        public static ContextModel ModelDBLike = null;
        public static ContextModel ModelDB = null;
        public static ContextModel ModelDBComment = null;

        static void Main()
        {
            try
            {
                ModelDB = new ContextModel();// Intait DB Model.
                ModelDBLike = new ContextModel();// Intait DB Model.
                ModelDBComment = new ContextModel();// Intait DB Model.
                Console.ForegroundColor = ConsoleColor.Cyan; // Set the color of Console Font

                ShowOption(); // See Options
                
                int selectOp = 0;
                try
                {
                    Console.Write("Your Select: ");
                    selectOp = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                SelectChoice Sc = new SelectChoice(SelectView);
                switch (selectOp)
                {
                    case 1:
                        // This is Select for New Operation
                        StartNew(Sc.Invoke());
                        break;
                    case 2:
                        Console.WriteLine("Insert the Post ID");
                        while (true)
                        {
                            Console.Write("Post ID: ");
                            string postId = Console.ReadLine();
                            if (!string.IsNullOrEmpty(postId))
                            {
                                if (ModelDB.Data.Where(s => s.PostID == postId).Count() == 0)
                                {
                                    if (DeleteOption()){ break;} // See Delete Options
                                }
                                else
                                {
                                    //Delete The Operation and Data refer to it
                                    DeleteOperation(postId);
                                    break;
                                }
                            }
                            else
                            {  Console.WriteLine("Wrong ID or NULL\n Please Try again."); }
                        }
                        break;
                    case 3:
                        //Extract all the data from User Options.
                        Console.WriteLine("Hi, You can use this tools to Extract full data about the useres stored in your DB");
                        GetherDataOptions();
                        break;
                    default:
                        Console.WriteLine("Wrong Selection Please try Again..\n");
                        for (int i = 5; i > 0; i--)
                        {
                            Thread.Sleep(500);
                            Console.Write(" " + i);
                            Console.Beep();
                        }
                        Application.Restart();
                        break;
                }
            }
            catch
            {

            }

        }
        static void ShowOption()
        {
            Console.WriteLine("Enter Accsess Token");
            while (true)
            {
                Console.Write("Accsess: ");
                string accsess = Console.ReadLine();

                if (!string.IsNullOrEmpty(accsess))
                {
                    _operation = new Operation(accsess); // Intiat the Operation class [should do that to intai the accsess token]
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong Accsess, Is Null \n Please Try again.");
                }
            }


            Console.WriteLine("Hello Sir, This Is 200-OK Mangement Operation:");
            Console.WriteLine("1 - Add New Post ID");
            Console.WriteLine("2 - Delete Operation ID");
            Console.WriteLine("3 - Gethring Full Data from User");
        }
        static bool DeleteOption()
        {
            Console.WriteLine("Wrong ID, Not Found \n Please Try again.");
            Console.WriteLine("Try again Yes [Y] No [N]");
            Console.Write("Your Choice: ");
            string cw = Console.ReadLine();
            bool control = false;
            switch (cw)
            {
                case "N":
                case "n":
                    control = true;
                    break;
                case "Y":
                case "y":
                    Console.WriteLine("Nice..");
                    break;
                default:
                    Console.WriteLine("Wrong Selection Bay..\n");
                    for (int i = 5; i > 0; i--)
                    {
                        Thread.Sleep(250);
                        Console.Write(" " + i);
                        Console.Beep();
                        control = true;
                    }
                    break;
            }
            return control;
        }
        static void DeleteOperation(string postId)
        {
            //Delete The Operation and Data refer to it
            ModelDB.Data.Remove(ModelDB.Data.Where(s => s.PostID == postId).First());
            ModelDB.Likes.RemoveRange(ModelDB.Likes.Where(s => s.To == postId));
            ModelDB.Comments.RemoveRange(ModelDB.Comments.Where(s => s.PostID == postId));
            ModelDB.SaveChanges();
            Console.WriteLine("Deleted Succsessfully Bay..\n");
            for (int i = 5; i > 0; i--)
            {
                Thread.Sleep(250);
                Console.Write(" " + i);
                Console.Beep();
            }
            Application.Restart();
        }
        static int GetherFullData()
        {
            Console.WriteLine("1 - Full Data");
            Console.WriteLine("2 - Email & Mobile Phone");
            Console.WriteLine("3 - Interest [SOON]");
            Console.WriteLine();
            Console.Write("Your Choice: ");
            int choice = 0;
            string val = Console.ReadLine();
            if (!string.IsNullOrEmpty(val))
            {
                choice = int.Parse(val);
            }
            else
            {
                Console.WriteLine("Sorry, Wrong or NULL Value\nDo You need to try again/nYes [y] NO [n]");
                Console.Write("Your Choice: ");
                val = Console.ReadLine();
                if (!string.IsNullOrEmpty(val))
                {
                    switch (val)
                    {
                        case "Y":
                        case "y":
                            return GetherFullData();
                        case "N":
                        case "n":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Wrong Selection Please try Again..\n");
                            for (int i = 5; i > 0; i--)
                            {
                                Thread.Sleep(500);
                                Console.Write(" " + i);
                                Console.Beep();
                            }
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Selection Please try Again..\n");
                    for (int i = 5; i > 0; i--)
                    {
                        Thread.Sleep(500);
                        Console.Write(" " + i);
                        Console.Beep();
                    }
                    Environment.Exit(0);
                }
            }
            return choice;
        }
        static void GetherDataOptions()
        {
            //"1 - Full Data"
            //"2 - Email & Mobile Phone"
            //"3 - Interest [SOON]"

            // See the Information and Options
            bool con = true;
            while (con)
            {
                switch (GetherFullData())
                {
                    case 1:
                        new Thread(() =>
                        {
                            Scraper.FullDataScrap scraper = new Scraper.FullDataScrap();
                            scraper.Scrape();
                        }).Start();
                        con = false;
                        break;
                    case 2:
                        new Thread(() =>
                        {
                            Scraper.EmailPhoneScrape scraper = new Scraper.EmailPhoneScrape();
                            scraper.Scrape();
                        }).Start();
                        con = false;
                        break;
                    case 3:
                        new Thread(() =>
                        {
                           // Some Codes
                        }).Start();
                        con = false;
                        break;
                    default: Console.WriteLine("Wrong Selection Please try Again..\n");
                        break;
                }
                bool control = true;

                while (control)
                {
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("DO U Need to See the Progress?");
                    Console.WriteLine("Yes [y]\nNo [n]\nExit [e]");
                    Console.Write("Your Choice: ");
                    string cw = Console.ReadLine();
                    switch (cw)
                    {
                        case "N":
                        case "n":
                            break;
                        case "Y":
                        case "y":
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("Nice..");
                            Console.WriteLine();
                            Console.WriteLine("Number of Emails :" + ModelDB.Users.Where(s => s.Email != null).Count());
                            Console.WriteLine("Number of Mobile Phones :" + ModelDB.Users.Where(s => s.Phone != null).Count());
                            Console.WriteLine();
                            break;
                        case "e" : case "E":
                            control = false;
                            break;
                        default:
                            Console.WriteLine("Wrong Selection Bay..\n");
                            for (int i = 5; i > 0; i--)
                            {
                                Thread.Sleep(250);
                                Console.Write(" " + i);
                                Console.Beep();
                                control = false;
                            }
                            break;
                    }
                }
            }
        }
        private static string SelectView()
        {
            Console.WriteLine("Hello sir, Please Enter Your Post ID");
            
            while (true)
            {
                Console.Write("Post ID: ");
                string Pid = Console.ReadLine();
                if (!string.IsNullOrEmpty(Pid))
                {
                    return Pid;
                }
                else
                {
                    Console.WriteLine("Wrong ID or NULL\n Please Try again.");
                }
            }
        }
        static async void StartNew(string ID)
        {
            try
            {
                // Check if the Operation exsit before
                if (ModelDB.Data.Where(s => s.PostID.ToString() == ID).Count() == 0)
                {
                    try
                    {
                        //Create New Operation
                        ModelDB.Data.Add(new OperationData() { PostID = ID });
                        await ModelDB.SaveChangesAsync();
                        try
                        {
                            //Fire Thread for Gether Likes
                            new Thread(() =>
                            {

                                dynamic likes = _operation.Get(ID + "/likes?limit=100");
                                 FillLikesModel(ID, likes);
                            }).Start();

                        }
                        catch { }
                        try
                        {
                            //Fire Thread for Gether Comments
                            new Thread(() =>
                            {
                                dynamic comments = _operation.Get(ID + "/comments?limit=100");
                                 FillCommentsModel(ID, comments);
                            }).Start();
                            
                        }
                        catch { }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    Console.WriteLine("I fount this Operation started \n close[N]?");
                    Console.Write("Your Choice: ");
                    bool control = true;

                    while (control)
                    {
                        try
                        {
                            switch (Console.ReadLine())
                            {
                                case "N":
                                case "n":
                                    control = false;
                                    break;
                                default:
                                    Console.WriteLine("Wrong Choice.");
                                    Console.Beep();
                                    Console.Write("Your Choice: ");
                                    break;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch { }
        }
        //Gether Likes Data from Post
        static void FillLikesModel(string PostID, dynamic likes)
        {
            
            {
                try
                {
                    int Count = likes.data.Count;
                    for (int i = 0; i < Count; i++)
                    {
                        try
                        {
                            string lid = Guid.NewGuid().ToString(); // Like ID

                            string id = likes.data[i].id; //ID of User that Like the Post
                            string name = likes.data[i].name; // Name of this User
                            
                            //Save the Like Data
                            ModelDBLike.Likes.Add(new Likes()
                            {
                                To = PostID,
                                LikeID = lid
                            });
                            ModelDBLike.SaveChanges();

                            //Create User and assign Likes for its.
                            if (ModelDBLike.Users.Where(s => s.UserID == id).Count() == 0)
                            {
                                ModelDBLike.Users.Add(new Users() { UserID = id, FullName = name, Likes = new List<Likes>() { ModelDBLike.Likes.Where(s => s.LikeID == lid).First() } });
                                ModelDBLike.SaveChanges();
                            }
                            else
                            {
                                ModelDBLike.Users.Where(s => s.UserID == id).First().Likes.Add(ModelDBLike.Likes.Where(s => s.LikeID == lid).First());
                                ModelDBLike.SaveChanges();
                            }
                        }
                        catch { }
                    }
                    //Likes in case more than 100 Like, seperate for pages
                    //looping on this page and gether their likes.
                    string next = likes.paging.cursors.after;
                    if (next != null)
                    {
                        likes = _operation.Get(PostID + "/likes?after=" + next + "&limit=100");
                         FillLikesModel(PostID, likes);
                    }
                }
                catch { }
            }
        }
        //Comments in case more than 100 comments, sperate for many pages then looping on it and gether this data
        static async void NextRound(string comment_id, dynamic CommentsofComments)
        {
            try
            {
                string next = string.Empty;
                try
                {
                    next = CommentsofComments.paging.cursors.after;
                }
                catch { }

                if (!string.IsNullOrEmpty(next))
                {
                    CommentsofComments = _operation.Get(comment_id + "/comments?after=" + next + "&limit=100");
                    int _Count = CommentsofComments.data.Count;
                    if (_Count > 0)
                        await FillCommentsModel(comment_id, CommentsofComments);
                }
                else
                {
                    try
                    {
                        next = CommentsofComments.paging.next;
                    }
                    catch { }

                    if (!string.IsNullOrEmpty(next))
                    {
                        CommentsofComments = _operation.Get(comment_id + "/comments?after=" + next + "&limit=100");
                        int _Count = CommentsofComments.data.Count;
                        if (_Count > 0)
                            await FillCommentsModel(comment_id, CommentsofComments);
                    }
                }
            }
            catch { }
        }
        //Each Comment can have replay on it
        //This Method Gethering it.
        static async void NewRoundOfComment(string comment_id)
        {
            try
            {
                int CommentCounts = 0;
                dynamic CommentsofComments = _operation.Get(comment_id + "/comments?limit=100");
                CommentCounts = CommentsofComments.data.Count;
                for (int j = 0; j < CommentCounts; j++)
                {
                    try
                    {
                        await FillCommentsModel(comment_id, CommentsofComments);
                    }
                    catch { }

                    NextRound(comment_id, CommentsofComments);
                }
            }
            catch
            {

            }
        }
        //Gether Comments Data from Post
        static void FillCommentsModel(string PostID, dynamic comments)
        {
            {
                try
                {
                    int Count = comments.data.Count;
                    for (int i = 0; i < Count; i++)
                    {
                        try
                        {
                            //Generate the Owner Of Comments.
                            string from_id = string.Empty;
                            string from_name = string.Empty;
                            try
                            {
                                from_id = comments.data[i].from.id;
                                from_name = comments.data[i].from.name;
                            }
                            catch { }

                            //Generate the Structer of Comments and Likes on it.
                            Comment comment = new Comment();
                            string mess = string.Empty;
                            string comment_id = string.Empty;
                            try
                            {
                                mess = string.IsNullOrEmpty(comments.data[i].message) ? string.Empty : comments.data[i].message;
                                comment.Text = mess;
                                comment_id = comments.data[i].id;
                                comment.PostID = PostID;
                                comment.CommentID = comment_id;
                            }
                            catch { }

                            //Get Likes
                            dynamic likes = _operation.Get(comment_id + "/likes?limit=100");
                            //Create temporary model and fill it with likes on a specefic comment then extract the likes and put in Comments Strecture.
                            if (likes.data.Count > 0)
                                 FillLikesModel(comment_id, likes);

                            //Get Comments of Comments
                            NewRoundOfComment(comment_id);

                            //Assign Likes of Comments and save it.
                            ModelDBComment.Comments.Add(comment);
                            ModelDBComment.SaveChanges();
                            ModelDBComment.Comments.Where(s => s.CommentID == comment_id).First().Likes.AddRange(ModelDBComment.Likes.Where(s => s.To == comment_id));
                            ModelDBComment.SaveChanges();

                            if (ModelDBComment.Users.Where(s => s.UserID == from_id).Count() == 0)
                            {
                                ModelDBComment.Users.Add(new Users() { UserID = from_id, FullName = from_name,
                                    Comments = new List<Comment>() { ModelDBComment.Comments.Where(s => s.CommentID == comment_id).First() } });

                                ModelDBComment.SaveChanges();
                            }
                            else
                            {
                                ModelDBComment.Users.Where(s => s.UserID == from_id).First()
                                    .Comments.Add(ModelDBComment.Comments.Where(s => s.CommentID == comment_id).First());

                                ModelDBComment.SaveChanges();
                            }
                        }
                        catch { }
                    }
                    // Estaplish new round of Gether Comments if exist.
                    NextRound(PostID, comments);
                }
                catch
                {

                }
            }
        }
    }
}

