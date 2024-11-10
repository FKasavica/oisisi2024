using System;
using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.DAO
{
    public class IndexDAO : ISubject
    {
        private List<IObserver> _observers;
        private List<Models.Index> _indices;

        public IndexDAO()
        {
            _indices = new List<Models.Index>();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            return _indices.Count == 0 ? 1 : _indices.Max(i => i.Id) + 1;
        }

        public void Add(Models.Index index)
        {
            index.Id = NextId();
            _indices.Add(index);
            NotifyObservers();
        }

        public void Update(Models.Index updatedIndex)
        {
            var existingIndex = _indices.FirstOrDefault(i => i.Id == updatedIndex.Id);
            if (existingIndex != null)
            {
                existingIndex.MajorCode = updatedIndex.MajorCode;
                existingIndex.EnrollmentNumber = updatedIndex.EnrollmentNumber;
                existingIndex.EnrollmentYear = updatedIndex.EnrollmentYear;
                NotifyObservers();
            }
        }

        public void Remove(Models.Index index)
        {
            _indices.Remove(index);
            NotifyObservers();
        }

        public List<Models.Index> GetAll()
        {
            return _indices;
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
