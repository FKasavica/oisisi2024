﻿using System;
using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.DAO
{
    public class ProfessorDAO : ISubject
    {
        private List<IObserver> _observers;
        private List<Professor> _professors;

        public ProfessorDAO()
        {
            _professors = new List<Professor>();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            return _professors.Count == 0 ? 1 : _professors.Max(p => p.Id) + 1;
        }

        public void Add(Professor professor)
        {
            professor.Id = NextId();
            _professors.Add(professor);
            NotifyObservers();
        }

        public void Update(Professor updatedProfessor)
        {
            var existingProfessor = _professors.FirstOrDefault(p => p.Id == updatedProfessor.Id);
            if (existingProfessor != null)
            {
                existingProfessor.Surname = updatedProfessor.Surname;
                existingProfessor.Name = updatedProfessor.Name;
                existingProfessor.DateOfBirth = updatedProfessor.DateOfBirth;
                existingProfessor.PhoneNumber = updatedProfessor.PhoneNumber;
                existingProfessor.Email = updatedProfessor.Email;
                existingProfessor.PersonalIdNumber = updatedProfessor.PersonalIdNumber;
                existingProfessor.Title = updatedProfessor.Title;
                existingProfessor.YearsOfExperience = updatedProfessor.YearsOfExperience;
                existingProfessor.Subjects = updatedProfessor.Subjects;
                NotifyObservers();
            }
        }

        public void Remove(Professor professor)
        {
            _professors.Remove(professor);
            NotifyObservers();
        }

        public List<Professor> GetAll()
        {
            return _professors;
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}