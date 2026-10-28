import styles from "./AddButton.module.css";

function AddButton({ label = "Add", loadingLabel = "Saving...", onClick, disabled = false, loading = false, type = "button", expanded, controls }) {
    return (
        <button
            type={type}
            className={styles.button}
            onClick={onClick}
            disabled={disabled || loading}
            aria-busy={loading}
            aria-expanded={expanded}
            aria-controls={controls}
        >
            {loading ? loadingLabel : label}
        </button>
    );
}

export default AddButton;
