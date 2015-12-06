using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface Imovement {

	void makeMove(Vector3 direction);
	void configure(GameController controller);
	void rotateToDirection(Vector3 direction);
}

public interface IGameData
{
	int getLevelWidth();
	int getLevelLeght();
}

public interface ISectionDesigning
{
	List<GameObject> buildNewRow(List<GameObject> row);
}

public interface ISectionBuilderSelection
{
	ISectionBuilder selectNewSectionBuilder();
}

public interface ISectionBuilderConfiguration
{
	
}

public interface ISectionBuilder
{
	List<GameObject> buildNewRow(List<GameObject> row);
	int fromRow {get;set;}
	int toRow {get;set;}
	
	
}
