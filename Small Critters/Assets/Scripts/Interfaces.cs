﻿using UnityEngine;
using System.Collections;

public interface Imovement {

	void makeMove(Vector3 direction);
	void configure(GameController controller);
	void rotateToDirection(Vector3 direction);
}
