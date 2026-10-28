---
name: filter-button
description: Creates reusable filter buttons for React using JavaScript and CSS
---

# Filter Button Skill

## Purpose

This skill helps create reusable filter buttons for a React frontend application.

## Technology Stack

- React
- Vite
- JavaScript (JSX)
- CSS Modules

## Component rules

1. Use functional React components.
2. Create reusable filter button components.
3. Use props to pass data to components.
4. Use useState to manage the selected filter when locaql state is appropriate.
5. Use OnClick to handle filter selection.
6. Keep filtering logic separate from button presentation.
7. Do not introduce additional libraries unless necessary.

## Styling Rules

1. Use ASS Modules for styling.
2. Create a FilterButton.module.css file.
3. Import CSS Modules as `styles`.
4. Do not use inline styles unless necessary.
5. Follow the existing applications design.

## Button States

Every filter button must support:
- Default state
- Hover state
- Active stated
- Disabled state when needed

## Accessibility

1. Use the HTML button element.
2. Set type="button"
3. Use aria-pressed for toggle-style filter buttons.
4. Provide visible focus indicators.
5. Ensure sufficient tekst and background contrast.

## Responsive Design

1. Buttons must work on desktop and mobile.
2. Use flexible layouts when displaying multiple filters.
3. Avoid fixed widths unless necessary.
4. Prevent text from overflowing buttons.

## Workflow

When the user requests a filter button:

1. Inspect the existing React project structure.
2. Check whether a filter button component already exists.
3. Identify which data needs to be filtered.
4. Understand the existinf design and styling conventions.
5. Reuse existing components when possible.
6. Create or update the FilterButton component.
7. Use CSS Modules for styling.
8. Implement the filtering logic in the parent component.
9. Ensure only matching items are displayed.
10. Handle the case when no items match the filter.
11. Run available lint and build checks.
12. Explain the implemented changes.

## Restrictions

- Do not modify backend code
- Do not install additional libraries without a clear reason
- Do not rewrite unrelated components
- Do not change existing functionality unless requested
- Do not claim tests passed unless they were executed
