# MeshVertexBrush
Mesh의 Vertex에 색을 칠할 수 있는 Unity Asset 입니다. Vertex마다 Color 값을 지정할 수 있을 뿐만 아니라 Color 값이 있는 Vertex를 기반으로 새로운 Mesh를 생성할 수 있습니다.

Read this in other languages: 한국어, [English](README.md)

## 작업 순서
- 작업할 Mesh를 선택한 후 Asset에 포함되어 있는 VertexColor Material을 추가합니다. 해당 Mesh는 반드시 Collider가 있어야합니다.
 
  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373016/069697bc-dd9f-11e5-93c6-04cd985b4517.png)

- 'Mesh Vertex Brush' Component를 추가 합니다. 그리고 'Painting Mode Off'를 클릭하여 'Painting Mode On'으로 변경합니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373009/06512cc2-dd9f-11e5-9e8e-495f291cc02b.png)

- 선택된 Mesh 위에 마우스를 드래그 하여 색을 칠합니다. 색은 'Mesh Vertex Brush'의 Color 속성을 이용하여 바꿀 수 있습니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373010/0673aebe-dd9f-11e5-9957-317d4f0eeec2.png)

- 'Create Painted Mesh'를 클릭하여 색이 칠해진 Vertex를 기반으로 새로운 Mesh를 생성합니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373011/068bedbc-dd9f-11e5-99d1-6a524f2699db.png)

- 생성된 Mesh입니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373014/068dfaa8-dd9f-11e5-971b-9eebd2510110.png)

- 기존 Mesh를 선택하여 'Clear'를 클릭하여 칠했던 색을 정리합니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373015/068de928-dd9f-11e5-81b6-e5d7b4f5d3a6.png)

- 'Clear'된 상태입니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373012/068dbf84-dd9f-11e5-8516-9715169d78df.png)

- 처음에 추가했던 Material을 삭제합니다.

  ![alt tag](https://cloud.githubusercontent.com/assets/6466389/13373013/068dc7ea-dd9f-11e5-9206-d010463452ca.png)


## 저자
- 김대희(zsaladinz@gmail.com)

## 라이센스
- 해당 Asset은 MIT 라이센스를 따릅니다.
