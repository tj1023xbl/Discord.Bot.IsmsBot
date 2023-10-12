import { createTheme, Table, TableBody, TableCell, Input, TableContainer, TableHead, TableRow, FormControl, InputLabel, TextField, textFieldClasses, InputAdornment, typographyClasses, OutlinedInput, outlinedInputClasses, CircularProgress, IconButton, Icon, inputBaseClasses } from "@mui/material"
import { styled } from '@mui/material/styles'
import { Backspace, Clear } from '@mui/icons-material'
import styleVariables from '../variables.module.scss'
import Saying from "../data/models/Saying"
import { SetStateAction, useCallback, useState } from "react";
import styles from './IsmTable.module.scss'
import moment from 'moment'

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

const StyledFilterInput = styled(TextField, {
    shouldForwardProp: () => true,
    label: 'styled-filter-input',

})({
    '& .focused fieldset': {
        borderColor: ` ${styleVariables.yellow_dark} !important`
    },
    '& .root .outline': {
        borderColor: 'whitesmoke',
    },
    '.outline:hover ': {
        borderColor: 'gold'
    },
    'label.Mui-focused': {
        color: styleVariables.yellow_dark
    },
    '& input':{
        color: 'whitesmoke'
    }

})


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
                <StyledFilterInput InputProps={{ endAdornment: filter.length > 0 ? <Adornment setFilter={setFilter} /> : null, classes: { root: 'root', focused: 'focused', input: 'imput', notchedOutline: 'outline' } }} className={styles.filterTextField} onChange={(e) => { setFilter(e.target.value) }} value={filter} label='Filter' variant='outlined' />
                <Table className={styles.ismTable}>
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
                                    <TableCell className={styles.date}>{moment(saying.dateCreated).format("MMM Do 'YY")}</TableCell>
                                    <TableCell>{saying.ismRecorder}</TableCell>
                                </TableRow>
                            )
                        })}
                    </TableBody>
                </Table>
            </>
            :
            <div className={styles.loading} >
                <CircularProgress size={64} />
            </div>
    )
}
