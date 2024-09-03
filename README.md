# Inheritance-Edtor
Unity Engine에서 특정 태그, 컴포넌트 등 특성으로 상속관계를 쉽게 지정하는 커스텀 에디터

# 패키지 다운로드 링크
---추가예정---

# 개발 환경
- Unity Engine 2023.2.16f1
- Visual Studio
- C#

# 기술 스택
- Unity Engine의 커스텀 에디터 기능
- EditorWindow
- GUILayer

# 개요
1. 레벨 디자인 과정에서 많은 Object가 Hierarchy에 난잡하게 배치되어 있을 때, 일일히 드래그드롭으로 부모-자식 오브젝트를 상속시키거나 Hierarchy의 정리가 필요한 경우가 있었다. 이에 필요성을 느껴, 상속 자동화 기능을 구현하였다.

# 기능
1. Hierarchy에서 특정 Tag 또는 Component를 가진 자식 오브젝트 하나 혹은 여러개를 지정한 부모 오브젝트에 상속시킬 수 있음.
2. Tag, Component로 자식 오브젝트를 필터링할 수 있다.

# 사용 예시
  ## Tool -> InheritanceEditor 클릭
![ToolBar2](https://github.com/user-attachments/assets/e6b0992c-501f-490b-a940-a514a5123b4f)
![editor](https://github.com/user-attachments/assets/820089c8-c9cf-48c1-b77a-730913ae7acb)
 
  ## Parent Object(부모) : 부모가 될 오브젝트 선택
  ![setparent](https://github.com/user-attachments/assets/6d006df4-df77-4f6d-bf7f-9ea3e2071241)
 
  ## Filter By Tag(태그로 지정) : 특정 태그를 가진 오브젝트를 자식으로 설정하고자 할 때 
  ![filterbytag](https://github.com/user-attachments/assets/9c111e9c-bb25-4a59-9c87-1aade3ad84f4)
 
  ## Component Type(컴포넌트로 지정) : 특정 컴포넌트를 가진 오브젝트를 자식으로 설정하고자 할 때. 컴포넌트 명을 입력.
  ![setChild](https://github.com/user-attachments/assets/35ecd077-a38c-44cb-9b9f-485538d6282d)
 
  ## Inheritance(상속 수행) : 클릭 시 선택한 부모 오브젝트에게 자식 오브젝트가 상속됨.
  ![상속수행](https://github.com/user-attachments/assets/078503e4-da24-45db-ae87-01c01e297cce)

  ## 상속 정상 수행 시 콘솔 출력
  ![console](https://github.com/user-attachments/assets/c5f7638a-906a-4fbb-8c33-16564135a8c4)

  ## 컴포넌트명 미입력 시
  ![컴포넌트낫파운드](https://github.com/user-attachments/assets/07053709-3f29-4008-9d97-f6f34ee57055)

  ## 태그 미선택 시
  ![태그명미선택시](https://github.com/user-attachments/assets/659c8a70-5880-4cb1-a5fa-738d35cbf28d)

# 추가 예정 기능
1. Undo 기능 : Undo.RecordObject를 사용한 상속 해제 기능
2. 커스텀 필터링 기능 : 레이어를 사용한 필터링 기능 

# 업데이트 내역
 - 2024.09.03 Ver.1

