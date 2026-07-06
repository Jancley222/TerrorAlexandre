using UnityEngine;

public interface ICreditMover
{
    void Move(RectTransform rectTransform, float speed, float deltaTime);
}