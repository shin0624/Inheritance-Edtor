using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InheritanceEditor : EditorWindow
{
    private GameObject parentObject; // 부모 오브젝트를 지정할 필드
    private bool filterByTag; // 태그로 필터링할지, 컴포넌트로 필터링할지 선택할 플래그
    private string selectedTag; // 태그로 필터링 할 경우 선택할 태그
    private string componentTypeName; // 컴포넌트로 필터링 할 경우 선택할 컴포넌트 타입

    [MenuItem("Tools/InheritanceEditor")]
    public static void ShowWindow() // 에디터 윈도우 오픈
    {
        InheritanceEditor window = (InheritanceEditor)GetWindow(typeof(InheritanceEditor), false, "InheritanceEditor");

        //윈도우 크기 설정
        window.minSize = new Vector2(400, 300);
        window.maxSize = new Vector2(800, 500);

    }

    private void OnGUI()
    {
        // 부모 오브젝트를 설정하는 필드
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object(부모)", parentObject, typeof(GameObject), true);

        // 필터링 방식을 선택(태그 또는 컴포넌트)
        filterByTag = EditorGUILayout.Toggle("Filter By Tag(태그로 지정)", filterByTag);
        if (filterByTag)
        {
            // 태그를 선택할 드롭다운 메뉴
            selectedTag = EditorGUILayout.TagField("Select Tag(태그선택)", selectedTag);
        }
        else
        {
            // 컴포넌트 타입을 입력하는 필드
            componentTypeName = EditorGUILayout.TextField("Component Type(컴포넌트로 지정)", componentTypeName);
        }

        // 적용 버튼
        if (GUILayout.Button("Inheritance(상속 수행)"))
        {
            AttachObjects(); // 버튼을 누르면 기능 실행
        }
    }

    private void AttachObjects()
    {
        // 부모 오브젝트가 설정되지 않았다면 경고 메시지 표시
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object is not assigned.");
            return;
        }

        if (filterByTag)
        {
            // 태그로 필터링할 경우
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(selectedTag); // 복수형 사용

            foreach (var obj in objectsWithTag)
            {
                // 각 오브젝트를 부모 오브젝트의 자식으로 설정
                obj.transform.SetParent(parentObject.transform);
            }

            Debug.Log($"{objectsWithTag.Length} objects with tag '{selectedTag}' have been attached to {parentObject.name}.");
        }
        else
        {
            // 입력된 컴포넌트 타입으로 필터링할 경우
            var componentType = System.Type.GetType(componentTypeName);

            if (componentType == null)
            {
                Debug.LogError($"Component Type '{componentTypeName}' not found.");
                return;
            }

            // 해당 컴포넌트를 가진 모든 오브젝트를 찾음
            Component[] components = (Component[])FindObjectsOfType(componentType);//오브젝트타입을 컴포넌트 타입으로 캐스트
            foreach (var component in components)
            {
                // 각 컴포넌트의 오브젝트를 부모 오브젝트의 자식으로 설정
                component.gameObject.transform.SetParent(parentObject.transform);
            }

            Debug.Log($"{components.Length} objects with component '{componentTypeName}' have been attached to {parentObject.name}.");
        }
    }
}
