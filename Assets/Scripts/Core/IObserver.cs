using UnityEngine;
using System;
using System.Collections.Generic;
	public interface IObserver
	{
        void Notifyed<T>(int id,T data);
	}