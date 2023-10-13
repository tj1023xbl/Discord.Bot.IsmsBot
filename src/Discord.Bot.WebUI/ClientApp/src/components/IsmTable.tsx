import { Table, TableBody, TableCell, TableHead, TableRow, TextField, CircularProgress } from "@mui/material"
import { styled } from '@mui/material/styles'
import styleVariables from '../variables.module.scss'
import Saying from "../data/models/Saying"
import { useCallback, useState } from "react";
import styles from './IsmTable.module.scss'
import moment from 'moment'
import Adornment from "./Adornment"
import EditModal from "./edit-modal/EditModal";
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment'

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
    '& input': {
        color: 'whitesmoke'
    }

})


export const IsmTable = ({ sayings }: { sayings: Saying[] }) => {

    const [filter, setFilter] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [activeSaying, setActiveSaying] = useState<Saying | null>(null)

    const openSayingEditModal = (saying: Saying) => {
        setActiveSaying(saying);
        setModalOpen(true);
    }

    const handleModalClose = () => {
        setActiveSaying(null);
        setModalOpen(false);

    }

    const handleFilter = useCallback((saying: Saying) => {
        return saying.ismKey.toLowerCase().includes(filter.toLowerCase()) ||
            saying.ismRecorder.toLowerCase().includes(filter.toLowerCase()) ||
            saying.ismSaying.toLowerCase().includes(filter.toLowerCase())
    }, [filter])


    return (
        sayings.length > 0 ?
            <LocalizationProvider dateAdapter={AdapterMoment}>
                {
                    activeSaying !== null ?
                        <>
                            <h1>asdasdasdasd</h1>
                            < EditModal
                                isModalOpen={modalOpen}
                                saying={activeSaying}
                                handleClose={handleModalClose}
                                ismKeyList={new Set(sayings.map(s => s.ismKey))}
                                recorderList={new Set(sayings.map(s => s.ismRecorder))}
                            />
                        </> : null
                }
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
                                <TableRow key={saying.id} onClick={() => openSayingEditModal(saying)} style={{ cursor: 'pointer' }}>
                                    <TableCell>{saying.ismSaying}</TableCell>
                                    <TableCell>{saying.ismKey}</TableCell>
                                    <TableCell className={styles.date}>{moment(saying.dateCreated).format("MMM Do 'YY")}</TableCell>
                                    <TableCell>{saying.ismRecorder}</TableCell>
                                </TableRow>
                            )
                        })}
                    </TableBody>
                </Table>
            </LocalizationProvider> :
            <div className={styles.loading} >
                <CircularProgress size={64} />
            </div>
    )
}
