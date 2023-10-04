import { createTheme, Paper, styled, Table, TableBody, TableCell, tableCellClasses, TableContainer, tableContainerClasses, TableHead, TableRow } from "@mui/material"
import styleVariables from '../variables.module.scss'
import Saying from "../data/models/Saying"
import { ThemeProvider } from "@emotion/react";


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

const StyledTableCell = styled(TableCell)({
    [`&.${tableCellClasses.head}`]: {
        backgroundColor: '#303030',
        color: 'whitesmoke',
        //fontWeight: 600,
        fontSize: '1.5em',
        borderTop: `1px solid ${styleVariables.yellow_light}`
    },
    [`&.${tableCellClasses.body}`]: {
        backgroundColor: '#505050',
        color: 'whitesmoke'
    }
})


export const IsmTable = ({ sayings }: { sayings: Saying[] }) => {



    return (
        <>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <StyledTableCell>Saying</StyledTableCell>
                            <StyledTableCell>Key</StyledTableCell>
                            <StyledTableCell>DateTime</StyledTableCell>
                            <StyledTableCell>Recorder</StyledTableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {sayings.map((saying) => {
                            return (
                                <TableRow key={saying.id}>
                                    <StyledTableCell>{saying.ismSaying}</StyledTableCell>
                                    <StyledTableCell>{saying.ismKey}</StyledTableCell>
                                    <StyledTableCell>{saying.dateCreated.toString()}</StyledTableCell>
                                    <StyledTableCell>{saying.ismRecorder}</StyledTableCell>
                                </TableRow>
                            )
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    )
}
