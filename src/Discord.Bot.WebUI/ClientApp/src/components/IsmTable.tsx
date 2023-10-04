import { createTheme, Paper, styled, Table, TableBody, TableCell, tableCellClasses, TableContainer, TableHead, TableRow, tableRowClasses } from "@mui/material"
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

const customTheme = createTheme({
    components: {
        MuiTableRow: {
            styleOverrides: {
                head: {
                    backgroundColor: `#101010 !important`,
                    ['&:hover']: {
                        backgroundColor: `#101010 !important`,
                    }

                },
                root: {
                    backgroundColor: '#404040',
                    ['&:nth-of-type(odd)']: {
                        backgroundColor: '#303030',
                    },
                    ['&:hover']: {
                        backgroundColor: '#202020 !important',
                    }
                },
            },
        },
        MuiTableCell: {
            styleOverrides: {
                root: {
                    color: `whitesmoke`
                },
                head: {
                    color: `${styleVariables.yellow_dark} !important`,
                }
            }
        }
    }
})

export const IsmTable = ({ sayings }: { sayings: Saying[] }) => {



    return (
        <>
            <ThemeProvider theme={customTheme}>
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
                            {sayings.map((saying) => {
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
            </ThemeProvider>
        </>
    )
}
