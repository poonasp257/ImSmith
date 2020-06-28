using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Util {
	public static void Swap<T>(ref T lhs, ref T rhs) {
		T temp = lhs;
		lhs = rhs;
		rhs = temp;
	}

	public static void Swap<T>(List<T> list, int i, int j) {
		T tmp = list[i];
		list[i] = list[j];
		list[j] = tmp;
	}

	public static void CreateJsonFile(string directory, string fileName, string jsonData) {
		string path = Application.streamingAssetsPath + string.Format("{0}/{1}.json", directory, fileName);
		var streamWriter = new StreamWriter(path);
		streamWriter.Write(jsonData);
	}

	public static string LoadJsonFile(string directory, string fileName) {
		string path = Application.streamingAssetsPath + string.Format("{0}/{1}.json", directory, fileName);
		var streamReader = new StreamReader(path);
		var jsonData = streamReader.ReadToEnd();
		return jsonData;
	}
}