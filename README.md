# Inheritance-Edtor
Unity Engine에서 특정 태그, 컴포넌트, 레이어 등 특성으로 상속관계를 쉽게 지정하는 커스텀 에디터

# 패키지 다운로드 링크
https://drive.google.com/file/d/1d9O2hfBgHAFYc8q9Na02tbALlXJ2rcZp/view?usp=sharing

# 개발 환경
- Unity Engine 2023.2.16f1
- Visual Studio
- C#

# 기술 스택
- Unity Engine의 커스텀 에디터 기능
- EditorWindow
- EditorGUILayer / GUILayer
  
# 개요
1. 레벨 디자인 과정에서 많은 Object가 Hierarchy에 난잡하게 배치되어 있을 때, 일일히 드래그드롭으로 부모-자식 오브젝트를 상속시키거나 Hierarchy의 정리가 필요한 경우가 있었다. 이에 필요성을 느껴, 상속 자동화 기능을 구현하였다.

# 기능
1. Hierarchy에서 특정 Tag, Layer 또는 Component를 가진 자식 오브젝트 하나 혹은 여러개를 지정한 부모 오브젝트에 상속시킬 수 있음.
2. Tag, Component, Layer로 자식 오브젝트를 필터링할 수 있다.
3. 전체 상속 해제, 개별 상속 해제가 가능하다.
4. 상속관계 현황을 결과 화면으로 확인할 수 있다.

# 사용 예시
  ## Tool -> InheritanceEditor 클릭
  ![ToolBar2](https://github.com/user-attachments/assets/e6b0992c-501f-490b-a940-a514a5123b4f)
  ![에디터최선](https://github.com/user-attachments/assets/4cba9a48-b8a7-456c-b17b-b52e48541a86)
 
  ## Parent Object(부모) : 부모가 될 오브젝트 선택
  ![부모가 될 오브젝트 선택](https://github.com/user-attachments/assets/f6328a0e-ebb3-4489-818f-56649e6281d4)
 
  ## Filter By Tag(태그로 지정) : 특정 태그를 가진 오브젝트를 자식으로 설정하고자 할 때 
  ![태그로 지정](https://github.com/user-attachments/assets/7ff7f44e-ec51-45e1-941c-abd2b566d337)
 
  ## Component Type(컴포넌트로 지정) : 특정 컴포넌트를 가진 오브젝트를 자식으로 설정하고자 할 때. 스크립트 컴포넌트 명을 입력.
  ![컴포넌트로 지정](https://github.com/user-attachments/assets/d6cb15d6-e06f-4216-b8dc-5cc4360cf559)

  ## Filter By Layer(레이어로 지정) : 특정 레이어로 지정된 오브젝트를 자식으로 설정하고자 할 때
  ![레이어선택](https://github.com/user-attachments/assets/c1654ada-0b1e-43fa-8c1b-e06a2a10bc26)
  
  ## Inheritance(상속 수행) : 클릭 시 선택한 부모 오브젝트에게 자식 오브젝트가 상속됨.
  ![상속시](https://github.com/user-attachments/assets/0c373b29-0999-4f93-a60c-f2362ded3732)
    All Undo 클릭시 상속된 오브젝트 전체 상속 취소 / 상속 결과 창에서 오브젝트 별 상속 취소도 가능.

  ## 자식 오브젝트 버튼화 : 결과 목록 내 자식을 선택하면 하이어라키에서 해당 자식이 선택됨
  ![버튼화](https://github.com/user-attachments/assets/1c755631-70a5-46ae-93d2-8bbd4a1eefbf)
  
  ## 상속 정상 수행 시 콘솔 출력
  ![console](https://github.com/user-attachments/assets/c5f7638a-906a-4fbb-8c33-16564135a8c4)

  ## 컴포넌트명 미입력 시
  ![컴포넌트낫파운드](https://github.com/user-attachments/assets/07053709-3f29-4008-9d97-f6f34ee57055)

  ## 태그 미선택 시
  ![태그명미선택시](https://github.com/user-attachments/assets/659c8a70-5880-4cb1-a5fa-738d35cbf28d)

# 추가 예정 기능

# 버전 목록 
- 2024.09.03 Ver.1 : https://drive.google.com/file/d/1buOOMtjyE5hp1ioAiS9-7awTNe__Hqx2/view?usp=drive_link
- 2024.09.07 Ver.2 : https://drive.google.com/file/d/1lP7OZYbDDs45PjnZRVMMk7d11it2Mw-l/view?usp=drive_link
- 2024.09.27 Ver.3 : https://drive.google.com/file/d/1d9O2hfBgHAFYc8q9Na02tbALlXJ2rcZp/view?usp=sharing
  
# 업데이트 내역
 - Ver.1
     1. 특정 태그, 컴포넌트 명으로 상속관계를 쉽게 지정하는 커스텀 에디터 설계

 - Ver.2
     1. Undo 기능 구현을 위해 Undo.RecordObject를 사용하여 상속 전후 오브젝트 상태를 기록하고, 상속 취소 메서드를 작성
     2. 상속 결과를 쉽게 보여주기 위해 레이아웃 내 윈도우 영역을 배치, 결과 목록 내 자식들은 아이콘과 BoxStyle로 표현된 버튼으로 구현
     3. 상속 결과 내 자식 버튼을 클릭하면 하이어라키에서 해당 오브젝트가 선택되도록 구현

 - Ver.3
     1. 레이어를 사용해 자식을 필터링하고 상속을 수행하는 Filter By Layer 기능 추가
     2. 자식 오브젝트 별로 각각 상속을 취소하는 기능 추가
