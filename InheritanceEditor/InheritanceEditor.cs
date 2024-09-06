using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InheritanceEditor : EditorWindow
{
    //24.09.06 업데이트
    // 1. Undo.RecordObject를 사용한 상속 취소기능 구현
    // 2. 상속 결과 디스플레이 구현 : Box와 IconContent를 사용하여 자식목록 시각화
    // 3. 상속 결과 자식 선택 시 하이어라키에서 함께 선택됨

    private GameObject parentObject; // 부모 오브젝트를 지정할 필드
    private bool filterByTag; // 태그로 필터링할지, 컴포넌트로 필터링할지 선택할 플래그
    private string selectedTag; // 태그로 필터링 할 경우 선택할 태그
    private string componentTypeName; // 컴포넌트로 필터링 할 경우 선택할 컴포넌트 타입

    private Vector2 scrollPosition;
    private Rect rtArea = new Rect(10, 150, 750, 300);

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
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object(부모 선택)", parentObject, typeof(GameObject), true);
        GUILayout.Space(10);//여백 추가

        // 필터링 방식을 선택(태그 또는 컴포넌트)
        filterByTag = EditorGUILayout.Toggle("Filter By Tag", filterByTag);
        if (filterByTag)
        {
            selectedTag = EditorGUILayout.TagField("Select Tag(태그 선택)", selectedTag);// 태그를 선택할 드롭다운 메뉴
        }
        else
        {
            componentTypeName = EditorGUILayout.TextField("Component Name", componentTypeName);// 컴포넌트 타입을 입력하는 필드
        }

        GUILayout.Space(10);

        // 적용 버튼
        if (GUILayout.Button("Inheritance(상속 수행)"))
        {
            AttachObjects(); // 버튼을 누르면 기능 실행
        }

        GUILayout.Space(10);
        
        EditorGUI.BeginDisabledGroup(parentObject == null || parentObject.transform.childCount == 0);//부모 오브젝트가 선택되지 않았거나, 부모 오브젝트에 상속된 자식 갯수가 0이면 비활성화.
        if (GUILayout.Button("Undo(상속 해제)"))//상속 해제 기능
        {
            DetachObject();
        }
        EditorGUI.EndDisabledGroup();

        // 상속 결과 디스플레이 출력
        GUILayout.BeginArea(rtArea, "Inheritance Result(상속 결과)", GUI.skin.window);
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(740), GUILayout.Height(250));//스크롤 영역 설정
            {
                ResultDisplay();
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    private void AttachObjects()// 상속 수행 메서드
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
                Undo.RecordObject(obj.transform, "Attach to Parent");//undo를 위한 기록을 추가
                obj.transform.SetParent(parentObject.transform);// 각 오브젝트를 부모 오브젝트의 자식으로 설정
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
                Undo.RecordObject(component.transform, "Attach to Parent");//undo를 위한 기록을 추가
                component.gameObject.transform.SetParent(parentObject.transform); // 각 컴포넌트의 오브젝트를 부모 오브젝트의 자식으로 설정
            }
            Debug.Log($"{components.Length} objects with component '{componentTypeName}' have been attached to {parentObject.name}.");
        }
    }

    private void DetachObject()// 상속 취소 메서드
    {
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object is not assigned.");
            return;
        }
        for (int i = parentObject.transform.childCount - 1; i >= 0; i--)// 부모 오브젝트의 자식 수 만큼 반복
        {
            Transform child = parentObject.transform.GetChild(i);//부모 오브젝트의 자식을 가져온다.
            Undo.RecordObject(child.transform, "Detatch from Parent");//실행 취소를 기록
            child.SetParent(null);//상속관계 해제
        }
    }

    private void ResultDisplay()// 상속 수행 결과를 표시하는 메서드
    {
        GUIStyle labelSize = new GUIStyle(GUI.skin.label);// GUIStyle을 사용하여 폰트 크기를 설정.(부모 오브젝트)
        labelSize.fontSize = 15;
        labelSize.fontStyle = FontStyle.Bold;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);//자식 오브젝트 박스 스타일 설정
        boxStyle.fontSize= 14;
        boxStyle.padding = new RectOffset(10,10,5,5);//박스 내부 여백 설정
        boxStyle.alignment = TextAnchor.MiddleCenter;

        if (parentObject == null)
        {
            GUILayout.Label("No Parent Object assigned.");
            return;
        }

        GUILayout.Space(10);

        GUILayout.Label($"Parent : {parentObject.name}      (Child Count : {parentObject.transform.childCount})", labelSize);// 부모 오브젝트 이름 출력

        GUILayout.BeginVertical();
        {
            if (parentObject.transform.childCount > 0)//부모 오브젝트의 자식이 존재하면 목록 출력
            {
                for (int i = 0; i < parentObject.transform.childCount; i++)//부모의 자식 수만큼 반복하며 
                {
                    Transform child = parentObject.transform.GetChild(i);//자식 오브젝트 목록 출력
                    GUILayout.ExpandWidth(true);
                    GUILayout.BeginHorizontal();//아이콘과 텍스트를 같은 줄에 표시하기 위해 수평 레이아웃 사용
                    {
                        DisplayIconAndButtons(child, boxStyle);// 아이콘과 버튼 출력 기능을 메서드로 분리
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

    private void DisplayIconAndButtons(Transform childObject, GUIStyle style )//아이콘과 자식 버튼을 출력하는 메서드
    {
        GameObject child = childObject.gameObject;// 자식 오브젝트를 게임오브젝트 타입의 child에게 할당
        System.Type childType = child.GetType();// child의 타입을 받아 저장
        GUIContent iconContent = null;//아이콘 초기화
        iconContent = EditorGUIUtility.IconContent("GameObject Icon");

        if(iconContent!=null)
        {
            GUILayout.Label(iconContent, GUILayout.Width(20), GUILayout.Height(20));//아이콘 표시
        }

        GUILayout.Space(5);

        if (GUILayout.Button($"- {child.gameObject.name}", style))//자식 오브젝트를 버튼 형식으로 박스 안에 삽입.
        {
            Selection.activeGameObject = child.gameObject;//클릭 시 하이어라키에 선택됨
        }
    }
}