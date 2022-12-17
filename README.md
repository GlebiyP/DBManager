# DBManager
 У проєкті реалізовано систему управління табличними СУБД.Cancel changes
Забеспечена підтримка наступних типів даних:
- integer
- real
- char
- string
- $ (грошовий форматний тип, max$ =10,000,000,000,000.00)
- $Invl

Реалізовано додаткову варіанту операцію - пошук (за шаблоном) та перегляд знайдених рядкiв таблицi.
# Виконані етапи:
- Етап 2. Розроблено власну версію СУТБД. Діаграма класів знаходиться у папці diagrams.
- Етап 10. Реалізовано операції над даними, орієнтуючись на їх ієрархічну структуру: база -> таблиця -> ... та на використання HTTP-запитів (GET, POST та DELETE).
- Етап 12. Розроблено OpenAPI Specification для взаємодії з ієрархічними даними (база, таблиця, ...) з використанням Swagger UI.
- Етап 18. Розроблено Web-проєкт із використанням ASP.NET Web API.
