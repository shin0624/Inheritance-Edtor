using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InheritanceEditor : EditorWindow
{
    private GameObject parentObject; // �θ� ������Ʈ�� ������ �ʵ�
    private bool filterByTag; // �±׷� ���͸�����, ������Ʈ�� ���͸����� ������ �÷���
    private string selectedTag; // �±׷� ���͸� �� ��� ������ �±�
    private string componentTypeName; // ������Ʈ�� ���͸� �� ��� ������ ������Ʈ Ÿ��

    [MenuItem("Tools/InheritanceEditor")]
    public static void ShowWindow() // ������ ������ ����
    {
        InheritanceEditor window = (InheritanceEditor)GetWindow(typeof(InheritanceEditor), false, "InheritanceEditor");

        //������ ũ�� ����
        window.minSize = new Vector2(400, 300);
        window.maxSize = new Vector2(800, 500);

    }

    private void OnGUI()
    {
        // �θ� ������Ʈ�� �����ϴ� �ʵ�
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object(�θ�)", parentObject, typeof(GameObject), true);

        // ���͸� ����� ����(�±� �Ǵ� ������Ʈ)
        filterByTag = EditorGUILayout.Toggle("Filter By Tag(�±׷� ����)", filterByTag);
        if (filterByTag)
        {
            // �±׸� ������ ��Ӵٿ� �޴�
            selectedTag = EditorGUILayout.TagField("Select Tag(�±׼���)", selectedTag);
        }
        else
        {
            // ������Ʈ Ÿ���� �Է��ϴ� �ʵ�
            componentTypeName = EditorGUILayout.TextField("Component Type(������Ʈ�� ����)", componentTypeName);
        }

        // ���� ��ư
        if (GUILayout.Button("Inheritance(��� ����)"))
        {
            AttachObjects(); // ��ư�� ������ ��� ����
        }
    }

    private void AttachObjects()
    {
        // �θ� ������Ʈ�� �������� �ʾҴٸ� ��� �޽��� ǥ��
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object is not assigned.");
            return;
        }

        if (filterByTag)
        {
            // �±׷� ���͸��� ���
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(selectedTag); // ������ ���

            foreach (var obj in objectsWithTag)
            {
                // �� ������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
                obj.transform.SetParent(parentObject.transform);
            }

            Debug.Log($"{objectsWithTag.Length} objects with tag '{selectedTag}' have been attached to {parentObject.name}.");
        }
        else
        {
            // �Էµ� ������Ʈ Ÿ������ ���͸��� ���
            var componentType = System.Type.GetType(componentTypeName);

            if (componentType == null)
            {
                Debug.LogError($"Component Type '{componentTypeName}' not found.");
                return;
            }

            // �ش� ������Ʈ�� ���� ��� ������Ʈ�� ã��
            Component[] components = (Component[])FindObjectsOfType(componentType);//������ƮŸ���� ������Ʈ Ÿ������ ĳ��Ʈ
            foreach (var component in components)
            {
                // �� ������Ʈ�� ������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
                component.gameObject.transform.SetParent(parentObject.transform);
            }

            Debug.Log($"{components.Length} objects with component '{componentTypeName}' have been attached to {parentObject.name}.");
        }
    }
}
