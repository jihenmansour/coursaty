using CoursesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoursesApp.Services
{

    public interface ITrainerService
    {
        int Create(Trainer trainer);
        Trainer FindByEmail(string email);
        Trainer ReadById(int Id);
        List<Trainer> ReadAll();
        bool Delete(int id);
        int Update(Trainer updatedTrainer);

    }
    public class TrainerService : ITrainerService
    {

        private readonly Courses_DBEntities1 db;
        public TrainerService()
        { 
            db = new Courses_DBEntities1();
        }
        public int Create(Trainer trainer)
        {
            trainer.Creation_Date= DateTime.Now;
            db.Trainers.Add(trainer);
            return db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var trainer = ReadById(id);

            if (trainer != null)
            {
                db.Courses.RemoveRange(db.Courses.Where(x => x.Trainer_Id == id));
                db.Trainers.Remove(trainer);
                return db.SaveChanges() > 0 ? true : false;
            }
            return false;
        }

        public Trainer FindByEmail(string email)
        {
           return db.Trainers.Where(t => t.Email == email).FirstOrDefault();
        }

        public List<Trainer> ReadAll()
        {
            return db.Trainers.ToList();
        }

        public Trainer ReadById(int Id)
        {
            return db.Trainers.Find(Id);
        }

        public int Update(Trainer updatedTrainer)
        {
            updatedTrainer.Creation_Date = DateTime.Now;
            db.Trainers.Attach(updatedTrainer);
            db.Entry(updatedTrainer).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }
    }
}