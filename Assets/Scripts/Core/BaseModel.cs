using UnityEngine;
using System;
using System.Collections.Generic;

	public class BaseModel : INotification
	{
		protected List<IObserver> listObservers = new List<IObserver>();
		private bool _hasUpdate = false;

		public void AddObserver(IObserver observer)
		{
			if (!listObservers.Contains(observer))
			{
				listObservers.Add(observer);
			}
		}
		public void RemoveObserver(IObserver observer)
		{
			if (listObservers.Contains(observer))
			{
				listObservers.Remove(observer);
			}
		}
		public void RemoveAllObserver()
		{
			listObservers.Clear();
		}
		public void NotifyObserver<T> (int id,T data)
		{
			_hasUpdate = true;
			foreach (IObserver observer in listObservers)
			{
                observer.Notifyed<T>(id, data);
			};
			this._hasUpdate = false;
		}
		public bool hasUpdate
		{
			get { return _hasUpdate; }
		}

	}