import { Backspace } from "@mui/icons-material";
import { IconButton, InputAdornment } from "@mui/material";
import { SetStateAction } from "react";
import styles from './IsmTable.module.scss'


const Adornment = ({ setFilter }: { setFilter: React.Dispatch<SetStateAction<string>> }) => {
    return (
        <InputAdornment position="end">
            <IconButton className={styles.clearButton} onClick={(e) => { setFilter('') }}>
                <Backspace />
            </IconButton>
        </InputAdornment>
    )
}

export default Adornment;