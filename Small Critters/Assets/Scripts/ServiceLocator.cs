using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ServiceLocator {

	private static Dictionary<object,object> services;

	public ServiceLocator()
	{
		services = new Dictionary<object, object> ();
	}

	public static void addService<T>(object serviceProvider)
	{
		services.Add (typeof(T), serviceProvider);
	}

	public static T getService<T>()
	{
		try
		{
			return (T)services[typeof(T)];
		}
		catch (KeyNotFoundException)
		{
			throw new UnityException("Requested service was not found in the service locator!");
		}
	}
}
