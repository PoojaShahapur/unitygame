#ifndef _MAINDIALOG_H
#define _MAINDIALOG_H

#include <QtWidgets/QDialog>
#include <iostream>
#include <QtCore/QTimer>
//#include "WorkThread.hxx"
//#include "ExcelTbl.hxx"

using namespace std;

namespace Ui
{
	class Dialog;
}

class MainDialog : public QDialog
{
	Q_OBJECT

public:
	MainDialog(QWidget *parent = 0);
	~MainDialog();

private:
	Ui::Dialog *ui;
	//ExcelTbl* m_excelTbl;
	//WorkThread m_thread;
	QTimer *m_timer;
    
private slots:
	void btnOutput();
	void btnXml();
	void btnStart();
//	void exitApp ();
	void update();

protected:
	void keyPressEvent(QKeyEvent *event);
};

#endif // MAINDIALOG_H
