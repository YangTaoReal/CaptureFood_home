using UnityEngine;
using System;
using System.Collections.Generic;
	public interface INotification
	{
		void AddObserver(IObserver observer);
		void RemoveObserver(IObserver observer);
		void NotifyObserver<T>(int id,T data);
	}