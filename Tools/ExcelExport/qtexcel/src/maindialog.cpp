#include "maindialog.hxx"
#include "ui_maindialog.h"
#include "Tools.hxx"

//#include <QtGui/QtGui>
#include <QtCore/QVector>
#include "CAppData.hxx"	// inlcude app data
#include "WorkThread.hxx"
#include "System.hxx"

MainDialog::MainDialog(QWidget *parent)
    : QDialog(parent, Qt::FramelessWindowHint), ui(new Ui::Dialog)
{
	ui->setupUi(this);

	Qt::WindowFlags flags = 0;
	flags |= Qt::WindowMinimizeButtonHint;
	flags |= Qt::WindowCloseButtonHint;
	this->setWindowFlags(flags); // 设置禁止最大化
	this->setFixedSize(629, 400); // 禁止改变窗口大小。
	this->setWindowIcon(QIcon(":/icons/icon.png"));	// 设置窗口图标

	connect(ui->pushButtonOutput, SIGNAL(clicked()), this, SLOT(btnOutput()));
	connect(ui->pushButtonOutputXml, SIGNAL(clicked()), this, SLOT(btnXml()));
	connect(ui->pushButtonStart, SIGNAL(clicked()), this, SLOT(btnStart()));
	setWindowTitle(QStringLiteral("Excel打包工具"));
	//this->setWindowFlags(Qt::WindowTitleHint);
	//this->setWindowFlags(Qt::WindowCloseButtonHint);

	// TODO: 设置控件属性 
	ui->comboBoxOutput->setInsertPolicy(QComboBox::InsertAtTop);
	ui->comboBoxOutput->setEditable(true);

	ui->comboBoxOutputXml->setInsertPolicy(QComboBox::InsertAtTop);
	ui->comboBoxOutputXml->setEditable(true);

	//ui->comboBoxSolution->setEditText("");
	ui->comboBoxSolution->setInsertPolicy(QComboBox::InsertAtTop);
	ui->comboBoxSolution->setEditable(true);

	// init appdata
	CAppData::getSingletonPtr()->initData();
	CAppData::getSingletonPtr()->initThread(new WorkThread());
	// fill to comboBoxSolution
	CAppData::getSingletonPtr()->initCombo(ui->comboBoxSolution);
	// clear select item,must after initCombo
	ui->comboBoxSolution->setCurrentIndex(-1);

	// TODO: 打表功能测试   
	//m_excelTbl = new ExcelTbl();
	//m_excelTbl = new ExcelTblSort();
	//m_thread.setParam(m_excelTbl);
	//m_thread.setParam(CAppData::getSingletonPtr()->getExcelTbl());
	Tools::getSingletonPtr()->setParent(this);
	//Tools::getSingletonPtr()->setTextEdit(ui->m_outTextEdit);
	// 检测大端小端
	System::getSingletonPtr()->checkEndian();

	m_timer = new QTimer(this);
	connect(m_timer, SIGNAL(timeout()), this, SLOT(update()));	// timeoutslot() 为自定义槽
	m_timer->start(1000);
}

MainDialog::~MainDialog()
{
	m_timer->stop();
	delete m_timer;
	//delete m_excelTbl;

	delete ui;
}

void MainDialog::btnOutput()
{
	QString dir = "";
	QString filter = "";
	if(ui->comboBoxOutput->currentText().length())
	{
		dir = ui->comboBoxOutput->currentText();
	}
	else
	{
		dir = QDir::currentPath();
	}

	QString fileName = Tools::getSingletonPtr()->openDirectoryDialog(dir);

    if (!fileName.isEmpty()) 
	{
		if (ui->comboBoxOutput->findText(fileName) == -1)
			ui->comboBoxOutput->addItem(fileName);
		ui->comboBoxOutput->setCurrentIndex(ui->comboBoxOutput->findText(fileName));
	}
}

void MainDialog::btnXml()
{
	QString dir = "";
	QString filter = "";
	// TODO: 默认过滤器    
	// filter = "All Files (*);;Text Files (*.txt)";
	filter = "All Files (*);;XML Files (*.xml)";
	if(ui->comboBoxOutputXml->currentText().length())
	{
		dir = ui->comboBoxOutputXml->currentText();
	}

	QString fileName = Tools::getSingletonPtr()->openFileDialog(dir, filter);

    if (!fileName.isEmpty()) 
	{
		if (ui->comboBoxOutputXml->findText(fileName) == -1)
		{
			ui->comboBoxOutputXml->addItem(fileName);
		}
		ui->comboBoxOutputXml->setCurrentIndex(ui->comboBoxOutputXml->findText(fileName));
	}
}

void MainDialog::btnStart()
{
	QString outPath = ui->comboBoxOutput->currentText();
	QString xmlFile = ui->comboBoxOutputXml->currentText();
	QString xmlsolution = ui->comboBoxSolution->currentText();

	CAppData::getSingletonPtr()->setXml(outPath.toLocal8Bit().data(), xmlFile.toLocal8Bit().data(), xmlsolution.toLocal8Bit().data());

	if((outPath.length() == 0 || xmlFile.length() == 0) && xmlsolution.length() == 0)
	{
		QString msg = tr("path is empty");
		Tools::getSingletonPtr()->informationMessage(msg);
	}
	else
	{
		//pushButtonStart.enable = false;

		//QMessageBox::information(this, tr("QMessageBox::information()"), tr("asdasdf"));
		if(!Tools::getSingletonPtr()->isRunning())
		{
			//m_excelTbl->setXmlPath(xmlFile);
			//m_excelTbl->setOutputPath(outPath);
			//m_thread.start();
			CAppData::getSingletonPtr()->startThread();
			//m_thread.wait();		// 会死锁 
			//pushButtonStart.enable = false;
		}
		else
		{
			//Tools::getSingletonPtr()->informationMessage(tr("正在打表"));
			Tools::getSingletonPtr()->informationMessage(QStringLiteral("正在打表"));
		}
	}
}

void MainDialog::update()
{
	QVector<QString>& list = Tools::getSingletonPtr()->getLog();
	QVector<QString>::Iterator begin = list.begin();
	while(begin != list.end())
	{
		//ui->m_outTextEdit->setPlainText(*begin);
		ui->m_outTextEdit->append(*begin);
		++begin;
	}

	// 显示对话框
	list = Tools::getSingletonPtr()->getLogMsg();
	begin = list.begin();
	if(begin != list.end())
	{
		Tools::getSingletonPtr()->informationMessageUI(*begin);
	}
}

void MainDialog::keyPressEvent(QKeyEvent *event)
{
	switch (event->key())
	{
		//进行界面退出，重写Esc键，否则重写reject()方法
	case Qt::Key_Escape:
		//this->close();
		break;

	default:
		QDialog::keyPressEvent(event);
	}
}