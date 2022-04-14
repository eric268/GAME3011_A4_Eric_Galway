using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    NUM_DIFFICULTY_LEVELS
}

public class DifficultyManager : MonoBehaviour
{
    [SerializeField]
    public int[] mDifficultyGridDimension = new int[3];
    public int mMaxSkillLevel = 10;
}
