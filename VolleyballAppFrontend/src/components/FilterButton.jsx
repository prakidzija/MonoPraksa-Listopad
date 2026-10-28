import styles from "./FilterButton.module.css";

function FilterButton({ children = "Filter", expanded, controls, disabled = false, onClick }) {
    return (
        <button
            type="button"
            className={styles.button}
            aria-expanded={expanded}
            aria-controls={controls}
            disabled={disabled}
            onClick={onClick}
        >
            {children}
        </button>
    );
}

export default FilterButton;
