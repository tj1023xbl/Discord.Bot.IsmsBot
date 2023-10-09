import { createTheme, Paper, Table, TableBody, TableCell, Input, TableContainer, TableHead, TableRow, FormControl, InputLabel, TextField, textFieldClasses, InputAdornment, typographyClasses, OutlinedInput, outlinedInputClasses, CircularProgress, IconButton, Icon } from "@mui/material"
import { Backspace, Clear } from '@mui/icons-material'
import styleVariables from '../variables.module.scss'
import Saying from "../data/models/Saying"
import { ThemeProvider } from "@emotion/react";
import { SetStateAction, useCallback, useState } from "react";
import styles from './IsmTable.module.scss'


type Order = 'asc' | 'desc';
function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
    if (b[orderBy] < a[orderBy]) {
        return -1;
    }
    if (b[orderBy] > a[orderBy]) {
        return 1;
    }
    return 0;
}

function getComparator<Key extends keyof any>(
    order: Order,
    orderBy: Key,
): (
    a: { [key in Key]: number | string },
    b: { [key in Key]: number | string },
) => number {
    return order === 'desc'
        ? (a, b) => descendingComparator(a, b, orderBy)
        : (a, b) => -descendingComparator(a, b, orderBy);
}



const Adornment = ({ setFilter }: { setFilter: React.Dispatch<SetStateAction<string>> }) => {
    return (
        <InputAdornment position="end">
            <IconButton className={styles.clearButton} onClick={(e) => { setFilter('') }}>
                <Backspace />
            </IconButton>
        </InputAdornment>
    )
}


export const IsmTable = ({ sayings }: { sayings: Saying[] }) => {

    const [filter, setFilter] = useState('');

    const handleFilter = useCallback((saying: Saying) => {
        return saying.ismKey.includes(filter) ||
            saying.ismRecorder.includes(filter) ||
            saying.ismSaying.includes(filter)
    }, [filter])

    return (
        sayings.length > 0 ?
            <>
                    <TextField className={ styles.filterTextField } onChange={(e) => { setFilter(e.target.value) }} value={filter} label='Filter' variant='outlined'
                        InputProps={{ endAdornment: filter.length > 0 ? <Adornment setFilter={setFilter} /> : null }} />
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>Saying</TableCell>
                                    <TableCell>Key</TableCell>
                                    <TableCell>DateTime</TableCell>
                                    <TableCell>Recorder</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {sayings.filter(handleFilter).map((saying) => {
                                    return (
                                        <TableRow key={saying.id}>
                                            <TableCell>{saying.ismSaying}</TableCell>
                                            <TableCell>{saying.ismKey}</TableCell>
                                            <TableCell>{saying.dateCreated.toString()}</TableCell>
                                            <TableCell>{saying.ismRecorder}</TableCell>
                                        </TableRow>
                                    )
                                })}
                            </TableBody>
                        </Table>
                    </TableContainer>
            </>
            :
            <div className={styles.loading} >
                <CircularProgress size={64} />
            </div>
    )
}
