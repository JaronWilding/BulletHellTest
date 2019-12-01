using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBehaviour)), CanEditMultipleObjects]
public class EnemyBehaviourEditor : Editor
{
    bool showPosition = false;

    SerializedProperty  score,
                        enemyHealth,
                        fireRate, bulletSpeed, fireAmount, rotationSpeed,
                        animSpeed, animReachDist, animRotSpeed, animUseBezier,
                        noDeath, noRotation, 
                        healthBar,
                        enemySprite, bulletSpawnPoint,
                        bulletPrefab, explosion, pooledAmount, dynamicList;

    private void OnEnable()
    {
        

        // Health Settings
        enemyHealth =       serializedObject.FindProperty("enemyHealth");
        score =             serializedObject.FindProperty("score");
        // Bullet Settings
        fireRate =         serializedObject.FindProperty("fireRate");
        bulletSpeed =       serializedObject.FindProperty("bulletSpeed");
        fireAmount =        serializedObject.FindProperty("fireAmount");
        rotationSpeed =     serializedObject.FindProperty("rotationSpeed");

        // Animation Settings
        animSpeed =         serializedObject.FindProperty("animSpeed");
        animReachDist =     serializedObject.FindProperty("animReachDist");
        animRotSpeed =      serializedObject.FindProperty("animRotSpeed");
        animUseBezier =     serializedObject.FindProperty("animUseBezier");

        // Debugging Options
        noDeath =           serializedObject.FindProperty("noDeath");
        noRotation =        serializedObject.FindProperty("noRotation");

        // Extra Health Settings
        healthBar =         serializedObject.FindProperty("healthBar");

        // Extra Animation Settings
        enemySprite =       serializedObject.FindProperty("enemySprite");
        bulletSpawnPoint =  serializedObject.FindProperty("bulletSpawnPoint");

        // Extra Bullet Settings
        bulletPrefab =      serializedObject.FindProperty("bulletPrefab");
        explosion =         serializedObject.FindProperty("explosion");
        pooledAmount =      serializedObject.FindProperty("pooledAmount");
        dynamicList =       serializedObject.FindProperty("dynamicList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EnemyBehaviour enemyBehaviour = (EnemyBehaviour)target;

        EditorGUILayout.LabelField("Enemy Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enemyHealth);
        EditorGUILayout.PropertyField(score);

        EditorGUILayout.LabelField("Bullet Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(fireRate);
        EditorGUILayout.PropertyField(bulletSpeed);
        EditorGUILayout.PropertyField(fireAmount);
        EditorGUILayout.PropertyField(rotationSpeed);

        EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(animSpeed);
        EditorGUILayout.PropertyField(animReachDist);
        EditorGUILayout.PropertyField(animRotSpeed);
        EditorGUILayout.PropertyField(animUseBezier);

        showPosition = EditorGUILayout.Foldout(showPosition, "Extra Settings", true);
        if (showPosition)
        {
            EditorGUILayout.LabelField("Debugging Options", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(noDeath);
            EditorGUILayout.PropertyField(noRotation);

            EditorGUILayout.LabelField("Extra Health Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(healthBar);

            EditorGUILayout.LabelField("Extra Animation Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(enemySprite);
            EditorGUILayout.PropertyField(bulletSpawnPoint);

            EditorGUILayout.LabelField("Extra Bullet Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(bulletPrefab);
            EditorGUILayout.PropertyField(explosion);
            EditorGUILayout.PropertyField(pooledAmount);
            EditorGUILayout.PropertyField(dynamicList);
        }

        serializedObject.ApplyModifiedProperties();

    }

}
