using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InheritanceEditor : EditorWindow
{
    //24.09.06 ������Ʈ
    // 1. Undo.RecordObject�� ����� ��� ��ұ�� ����
    // 2. ��� ��� ���÷��� ���� : Box�� IconContent�� ����Ͽ� �ڽĸ�� �ð�ȭ
    // 3. ��� ��� �ڽ� ���� �� ���̾��Ű���� �Բ� ���õ�

    private GameObject parentObject; // �θ� ������Ʈ�� ������ �ʵ�
    private bool filterByTag; // �±׷� ���͸�����, ������Ʈ�� ���͸����� ������ �÷���
    private string selectedTag; // �±׷� ���͸� �� ��� ������ �±�
    private string componentTypeName; // ������Ʈ�� ���͸� �� ��� ������ ������Ʈ Ÿ��

    private Vector2 scrollPosition;
    private Rect rtArea = new Rect(10, 150, 750, 300);

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
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object(�θ� ����)", parentObject, typeof(GameObject), true);
        GUILayout.Space(10);//���� �߰�

        // ���͸� ����� ����(�±� �Ǵ� ������Ʈ)
        filterByTag = EditorGUILayout.Toggle("Filter By Tag", filterByTag);
        if (filterByTag)
        {
            selectedTag = EditorGUILayout.TagField("Select Tag(�±� ����)", selectedTag);// �±׸� ������ ��Ӵٿ� �޴�
        }
        else
        {
            componentTypeName = EditorGUILayout.TextField("Component Name", componentTypeName);// ������Ʈ Ÿ���� �Է��ϴ� �ʵ�
        }

        GUILayout.Space(10);

        // ���� ��ư
        if (GUILayout.Button("Inheritance(��� ����)"))
        {
            AttachObjects(); // ��ư�� ������ ��� ����
        }

        GUILayout.Space(10);
        
        EditorGUI.BeginDisabledGroup(parentObject == null || parentObject.transform.childCount == 0);//�θ� ������Ʈ�� ���õ��� �ʾҰų�, �θ� ������Ʈ�� ��ӵ� �ڽ� ������ 0�̸� ��Ȱ��ȭ.
        if (GUILayout.Button("Undo(��� ����)"))//��� ���� ���
        {
            DetachObject();
        }
        EditorGUI.EndDisabledGroup();

        // ��� ��� ���÷��� ���
        GUILayout.BeginArea(rtArea, "Inheritance Result(��� ���)", GUI.skin.window);
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(740), GUILayout.Height(250));//��ũ�� ���� ����
            {
                ResultDisplay();
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    private void AttachObjects()// ��� ���� �޼���
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
                Undo.RecordObject(obj.transform, "Attach to Parent");//undo�� ���� ����� �߰�
                obj.transform.SetParent(parentObject.transform);// �� ������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
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
                Undo.RecordObject(component.transform, "Attach to Parent");//undo�� ���� ����� �߰�
                component.gameObject.transform.SetParent(parentObject.transform); // �� ������Ʈ�� ������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
            }
            Debug.Log($"{components.Length} objects with component '{componentTypeName}' have been attached to {parentObject.name}.");
        }
    }

    private void DetachObject()// ��� ��� �޼���
    {
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object is not assigned.");
            return;
        }
        for (int i = parentObject.transform.childCount - 1; i >= 0; i--)// �θ� ������Ʈ�� �ڽ� �� ��ŭ �ݺ�
        {
            Transform child = parentObject.transform.GetChild(i);//�θ� ������Ʈ�� �ڽ��� �����´�.
            Undo.RecordObject(child.transform, "Detatch from Parent");//���� ��Ҹ� ���
            child.SetParent(null);//��Ӱ��� ����
        }
    }

    private void ResultDisplay()// ��� ���� ����� ǥ���ϴ� �޼���
    {
        GUIStyle labelSize = new GUIStyle(GUI.skin.label);// GUIStyle�� ����Ͽ� ��Ʈ ũ�⸦ ����.(�θ� ������Ʈ)
        labelSize.fontSize = 15;
        labelSize.fontStyle = FontStyle.Bold;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);//�ڽ� ������Ʈ �ڽ� ��Ÿ�� ����
        boxStyle.fontSize= 14;
        boxStyle.padding = new RectOffset(10,10,5,5);//�ڽ� ���� ���� ����
        boxStyle.alignment = TextAnchor.MiddleCenter;

        if (parentObject == null)
        {
            GUILayout.Label("No Parent Object assigned.");
            return;
        }

        GUILayout.Space(10);

        GUILayout.Label($"Parent : {parentObject.name}      (Child Count : {parentObject.transform.childCount})", labelSize);// �θ� ������Ʈ �̸� ���

        GUILayout.BeginVertical();
        {
            if (parentObject.transform.childCount > 0)//�θ� ������Ʈ�� �ڽ��� �����ϸ� ��� ���
            {
                for (int i = 0; i < parentObject.transform.childCount; i++)//�θ��� �ڽ� ����ŭ �ݺ��ϸ� 
                {
                    Transform child = parentObject.transform.GetChild(i);//�ڽ� ������Ʈ ��� ���
                    GUILayout.ExpandWidth(true);
                    GUILayout.BeginHorizontal();//�����ܰ� �ؽ�Ʈ�� ���� �ٿ� ǥ���ϱ� ���� ���� ���̾ƿ� ���
                    {
                        DisplayIconAndButtons(child, boxStyle);// �����ܰ� ��ư ��� ����� �޼���� �и�
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.Space(10);
                GUILayout.Label("No Children.", boxStyle);
            }
        }
        GUILayout.EndVertical();
    }

    private void DisplayIconAndButtons(Transform childObject, GUIStyle style )//�����ܰ� �ڽ� ��ư�� ����ϴ� �޼���
    {
        GameObject child = childObject.gameObject;// �ڽ� ������Ʈ�� ���ӿ�����Ʈ Ÿ���� child���� �Ҵ�
        System.Type childType = child.GetType();// child�� Ÿ���� �޾� ����
        GUIContent iconContent = null;//������ �ʱ�ȭ
        iconContent = EditorGUIUtility.IconContent("GameObject Icon");

        if(iconContent!=null)
        {
            GUILayout.Label(iconContent, GUILayout.Width(20), GUILayout.Height(20));//������ ǥ��
        }

        GUILayout.Space(5);

        if (GUILayout.Button($"- {child.gameObject.name}", style))//�ڽ� ������Ʈ�� ��ư �������� �ڽ� �ȿ� ����.
        {
            Selection.activeGameObject = child.gameObject;//Ŭ�� �� ���̾��Ű�� ���õ�
        }
    }
}