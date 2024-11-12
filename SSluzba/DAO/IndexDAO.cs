using System;
using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;

namespace SSluzba.DAO
{
    public class IndexDAO : ISubject
    {
        private List<IObserver> _observers;
        private IndexRepository _repository;

        public IndexDAO()
        {
            _repository = new IndexRepository();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            var indices = _repository.LoadIndices();
            return indices.Count == 0 ? 1 : indices.Max(i => i.Id) + 1;
        }

        public void Add(Models.Index index)
        {
            var indices = _repository.LoadIndices();
            index.Id = NextId();
            indices.Add(index);
            _repository.SaveIndices(indices);
            NotifyObservers();
        }

        public void Update(Models.Index updatedIndex)
        {
            var indices = _repository.LoadIndices();
            var existingIndex = indices.FirstOrDefault(i => i.Id == updatedIndex.Id);
            if (existingIndex != null)
            {
                existingIndex.MajorCode = updatedIndex.MajorCode;
                existingIndex.EnrollmentNumber = updatedIndex.EnrollmentNumber;
                existingIndex.EnrollmentYear = updatedIndex.EnrollmentYear;
                _repository.SaveIndices(indices);
                NotifyObservers();
            }
        }

        public void Remove(Models.Index index)
        {
            var indices = _repository.LoadIndices();
            indices.RemoveAll(i => i.Id == index.Id);
            _repository.SaveIndices(indices);
            NotifyObservers();
        }

        public List<Models.Index> GetAll()
        {
            return _repository.LoadIndices();
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

        public int GetIndexId(string majorCode, int enrollmentNumber, int enrollmentYear)
        {
            var indices = _repository.LoadIndices();
            var index = indices.FirstOrDefault(i =>
                i.MajorCode == majorCode &&
                i.EnrollmentNumber == enrollmentNumber &&
                i.EnrollmentYear == enrollmentYear);

            return index != null ? index.Id : -1; // Vraća -1 ako indeks nije pronađen
        }

    }
}
