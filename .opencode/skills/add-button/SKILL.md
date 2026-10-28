---
name: add-button
description: Create reusable Add buttons in React and Vite applications using JavaScript, JSX and CSS Modules. Use when implementing buttons that initiate adding or creating new records, items or entities.
---

# Add Button Skill

## Purpose 

Create reuseable Add buttons that initiate creation of new items.

Examples:
- Add a volleyball player
- Add a volleyball club

## Technology stack

- React
- Vite
- JavaScript (JSX)
- CSS Modules

## Component Rules

1. Use functional React components.
2. Create reusable AddButton components.
3. Use props for button lables and click handlers.
4. Use onClick to trigger the add action.
5. Keep business logic outside AddButton component.
6. Use type="button"
7. Support disabled and loading states.
8. Prevent repeated submissions while an add request is pending.
9. Avoid unnecessary component abstractions.
10. Follow the existing frontend architecture.

## Reusable Button Beahvior

The AddButton component must be independant of the action performed when clicked

## Behavior Rules

1. The parent component controls the add action.
2. Do not hardcode navigation routes.
3. Do not hardcode API calls inside AddButton.
4. Do not hardcode modal behavior.
5. Use onClick prop to trigger the parent's action.
6. Disable the button while loading.
7. Prevent repeated clicks during loading.
8. Allow the button to be reused in different pages.
9. Display a loading indicator when appropriate.
10. Preserve existing application behavior.


## Styling Rules

1. Use CSS Modules for styling.
2. Create AddButton.module.css.
3. Import CSS Modules as `styles`.
4. Follow the existing application's design.
5. Provide default, hover, focus and disabled styles.
6. Avoid inline styles unless necessary.
7. Do not introduce Tailwind CSS or styled-components.

## Button States

Every Add button must support:

- Default state
- Hover state
- Keyboard focus state
- Disabled state
- Loading state

The loading state must prevent repeated actions.


## Accessibility

1. Use the semantic HTML button element.
2. Use type="button" by default.
3. Allow type="submit" when used inside forms.
4. Provide a clear accessible label.
5. Support keyboard activation.
6. Provide a visible focus indicator.
7. Ensure sufficient color contrast.
8. Disable the button during loading.

## Responsive Design

1. Buttons must work on desktop and mobile.
2. Avoid fixed widths unless necessary.
3. Prevent button text from overflowing.
4. Maintain comfortable touch target sizes.
5. Follow existing responsive breakpoints.

## Workflow

When the user requests an Add button:

1. Inspect the existing React project structure.
2. Check whether an AddButton or similar component exists.
3. Identify what entity the user wants to add.
4. Determine the intended action:
   - Open a modal
   - Navigate to a creation page
   - Execute a callback
5. Understand existing styling conventions.
6. Reuse existing components where appropriate.
7. Create or update the AddButton component.
8. Implement the action in the parent component.
9. Add CSS Module styling.
10. Handle disabled and loading states.
11. Verify keyboard accessibility.
12. Run available lint and build checks.
13. Explain the changes made.

## Restrictions

- Do not modify backend code unless explicitly requested.
- Do not introduce TypeScript.
- Do not install unnecessary dependencies.
- Do not rewrite unrelated components.
- Do not hardcode business logic inside AddButton.
- Do not assume an API request succeeded.
- Do not claim tests passed unless executed.